using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Dtos;
using SMEConnect.Modals;
using SMEConnect.Modals.JWTAuthentication.Authentication;
using static SMEConnect.Constatns.Constants;

namespace SMEConnect.Providers
{
    public class GroupUserProvider : IGroupUserProvider
    {
        private ILogger<GroupUserProvider> _logger;
        private DcimDevContext _dcimDevContext;
        private IAnnouncementProvider _announcementProvider;
        private IMapper _mapper;
        private IGroupUserRoleClaimProvider _groupUserRoleProvider;
        

        public GroupUserProvider(ILogger<GroupUserProvider> Logger, DcimDevContext dcimDevContext, IAnnouncementProvider announcementProvider,IMapper mapper,IGroupUserRoleClaimProvider groupUserRoleClaimProvider)
        {
            this._dcimDevContext = dcimDevContext;
            this._logger = Logger;
            _announcementProvider = announcementProvider;
            _mapper = mapper;
            _groupUserRoleProvider = groupUserRoleClaimProvider;
        }


        public async Task<ApiResponse<bool>> AddGroupUser(GroupUserRequestDto group)
        {
            try
            {
                bool exists = await _dcimDevContext.GroupUsers.AnyAsync(p => p.UserEmail == group.UserEmail && p.Group == group.Group);
                if (exists)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "A user with the same group already exists.");
                }

                var user = await _dcimDevContext.UserGroups.FirstAsync(u => u.Name == group.Group);

                var newAnn = new Announcement() { GroupName = group.Group, PracticeName = user.Practice, UserName = group.ModifiedBy, CreatedBy = group.ModifiedBy, Message = new AnnouncementMessages(group.Group, group.UserEmail).NewUserJoinedGroup};

                GroupUser newGroup = _mapper.Map<GroupUser>(group);
                await _dcimDevContext.GroupUsers.AddAsync(newGroup);
                await _dcimDevContext.SaveChangesAsync();

                var groupUserRoleClaims = new List<GroupUserRoleClaim>();

                foreach (var claimName in group.GroupRoleClaims)
                {
                    groupUserRoleClaims.Add(new GroupUserRoleClaim { GroupUserId = newGroup.Id, Claim = claimName, Id = 0 });
                }

                await _groupUserRoleProvider.CreateUpdateGroupClaim(groupUserRoleClaims);
                await _announcementProvider.AddAnnouncement(newAnn);
                await _dcimDevContext.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<List<GroupUser>>> GetUserGroups(string userEmail,string practice = "")
        {
            try
            {
                var user = await _dcimDevContext.Users
                    .FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user == null)
                {
                    return new ApiResponse<List<GroupUser>>(ApiResponseType.NotFound, new List<GroupUser>(), "User not found.");
                }

                var userPracticeGroupsQuery = _dcimDevContext.UserGroups.AsQueryable();

                if (!string.IsNullOrWhiteSpace(practice))
                {
                    userPracticeGroupsQuery = userPracticeGroupsQuery.Where(g => g.Practice == practice);
                }

                var userPracticeGroups = await userPracticeGroupsQuery.ToListAsync();

                var userJoinedGroups = await _dcimDevContext.GroupUsers
                    .Where(g => g.UserEmail == user.Email && userPracticeGroups.Select(p => p.Name).Contains(g.Group))
                    .ToListAsync();

                return new ApiResponse<List<GroupUser>>(ApiResponseType.Success, userJoinedGroups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user groups: {Message}", ex.Message);
                throw;
            }
        }



        public async Task<ApiResponse<List<GroupUser>>> getGroupUsers()
        {
            try
            {
                List<GroupUser> roles = await _dcimDevContext.GroupUsers.ToListAsync();
                await _dcimDevContext.SaveChangesAsync();
                return new ApiResponse<List<GroupUser>>(ApiResponseType.Success, roles);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<List<GroupUserDto>>> getGroupAllUsers(string group)
        {
            try
            {
                var usersWithNames = await _dcimDevContext.GroupUsers
                                    .Where(gu => gu.Group == group)
                                    .Join(
                                        _dcimDevContext.Users,
                                        gu => gu.UserEmail,
                                        u => u.Email,
                                        (gu, u) => new
                                        {
                                            gu.Id,
                                            gu.Group,
                                            gu.UserEmail,
                                            u.DisplayName,
                                            gu.GroupRole,
                                            gu.ModifiedOnDt,
                                            gu.ModifiedBy
                                        })
                                    .ToListAsync();

                var userDtos = usersWithNames.Select(u => new GroupUserDto
                {
                    Id = u.Id,
                    Group = u.Group,
                    UserEmail = u.UserEmail,
                    Name = u.DisplayName,
                    GroupRole = u.GroupRole,
                    ModifiedBy = u.ModifiedBy,
                    ModifiedOnDt = u.ModifiedOnDt

                }).ToList();

                return new ApiResponse<List<GroupUserDto>>(ApiResponseType.Success, userDtos);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }


        public async Task<ApiResponse<bool>> DeleteGroupUser(List<int> ids)
        {
            try
            {
                var userGroupsToRemove = _dcimDevContext.GroupUsers.Where(userGroup => ids.Contains(userGroup.Id)).ToList();

                if (userGroupsToRemove.Any())
                {
                    _dcimDevContext.GroupUsers.RemoveRange(userGroupsToRemove);
                    await _dcimDevContext.SaveChangesAsync();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                else
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "No matching user groups found.");
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<bool>> UpdateGroupUser(GroupUserRequestDto group)
        {
            try
            {
                var existingGroup = await _dcimDevContext.GroupUsers.FindAsync(group.Id);
                if (existingGroup == null)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "Selected group not found.");
                }

                existingGroup.UserEmail = group.UserEmail;
                existingGroup.Group = group.Group;
                existingGroup.GroupRole = group.GroupRole;
                existingGroup.ModifiedOnDt = DateTime.Now;

                var groupUserRoleClaims = new List<GroupUserRoleClaim>();

                foreach (var claimName in group.GroupRoleClaims)
                {
                    groupUserRoleClaims.Add(new GroupUserRoleClaim { GroupUserId = group.Id, Claim = claimName, Id = 0 });
                }

                await _groupUserRoleProvider.CreateUpdateGroupClaim(groupUserRoleClaims);
                 _dcimDevContext.GroupUsers.Update(existingGroup);
                await _dcimDevContext.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

    }

}

