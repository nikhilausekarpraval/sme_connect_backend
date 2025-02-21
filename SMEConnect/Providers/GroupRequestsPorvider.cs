using Microsoft.AspNetCore.Identity;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Dtos;
using SMEConnect.Modals;
using static SMEConnect.Constatns.Constants;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace SMEConnect.Providers
{

    public class GroupRequestsPorvider : IGroupRequestProvider
    {
        private DcimDevContext _decimDevContext;
        private ILogger<UserClaimProvider> _logger;
        private UserManager<ApplicationUser> _userManager;
        private IGroupUserProvider _groupUserProvider;

        public GroupRequestsPorvider(DcimDevContext dcimDevContext, UserManager<ApplicationUser> userManager, ILogger<UserClaimProvider> logger, IGroupUserProvider groupUserProvider)
        {
            this._decimDevContext = dcimDevContext;
            _logger = logger;
            _userManager = userManager;
            this._groupUserProvider = groupUserProvider;
        }

        public async Task<ApiResponse<List<GroupRequest>>> GetUserRequests(string userEmail)
        {
            try
            {
                var user = await _decimDevContext.Users.Where(u => u.Email == userEmail).FirstOrDefaultAsync();
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Contains("Admin") && user != null)
                {
                    var groupRequests = await _decimDevContext.GroupRequests.ToListAsync();
                    return new ApiResponse<List<GroupRequest>>(ApiResponseType.Success, groupRequests, "");

                }
                else
                {
                    var groupRequests = await (from groupRequest in _decimDevContext.GroupRequests
                                               join userGroups in _decimDevContext.UserGroups
                                                   on new { Practice = groupRequest.PracticeName, Group = groupRequest.GroupName }
                                                   equals new { Practice = userGroups.Practice, Group = userGroups.Name }
                                               join groupUsers in _decimDevContext.GroupUsers
                                                   on userGroups.Name equals groupUsers.Group
                                               where groupUsers.UserEmail == userEmail && groupUsers.GroupRole.ToLower() == "lead"
                                               select groupRequest).ToListAsync();

                    return new ApiResponse<List<GroupRequest>>(ApiResponseType.Success, groupRequests, "");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }
        }

        public async Task<ApiResponse<int>> GetUserRequestCount(string userEmail)
        {
            try
            {
                var user = await _decimDevContext.Users.Where(u => u.Email == userEmail).FirstOrDefaultAsync();
                var userRoles = await _userManager.GetRolesAsync(user);

                int requestCount;

                if (userRoles.Contains("Admin") && user != null)
                {
                    requestCount = await _decimDevContext.GroupRequests.Where(gr => gr.RequestStatus == false).CountAsync();
                }
                else
                {
                    requestCount = await (from groupRequest in _decimDevContext.GroupRequests
                                          join userGroups in _decimDevContext.UserGroups
                                              on new { groupRequest.PracticeName, groupRequest.GroupName }
                                              equals new { PracticeName = userGroups.Practice, GroupName = userGroups.Name }
                                          join groupUsers in _decimDevContext.GroupUsers
                                              on userGroups.Name equals groupUsers.Group
                                          where groupUsers.UserEmail == userEmail
                                                && groupUsers.GroupRole.ToLower() == "lead"
                                                && groupRequest.RequestStatus == false
                                          select groupRequest).CountAsync();

                }

                return new ApiResponse<int>(ApiResponseType.Success, requestCount, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }
        }

        public async Task<ApiResponse<UserRequestRoleCountDto>> getIsUserLeadForGroups(string userEmail)
        {
            try
            {
                bool isLead = await _decimDevContext.GroupUsers
                    .AnyAsync(gu => gu.UserEmail == userEmail && gu.GroupRole == "Lead");

                var count = 0;
                if (isLead)
                {
                    var result = await GetUserRequestCount(userEmail);
                    count = result.Data;
                }

                return new ApiResponse<UserRequestRoleCountDto>(ApiResponseType.Success, new UserRequestRoleCountDto() { isLead=isLead,requestCount = count});
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<bool>> UpdateUserRequests(GroupRequest groupRequest)
        {
            try
            {
                var group = await _decimDevContext.UserGroups.Where(ug => ug.Practice == groupRequest.PracticeName && ug.Name == groupRequest.GroupName).FirstOrDefaultAsync();

                var newGroupGroupRequest = new GroupUserRequestDto() { Id = group.Id, Group = groupRequest.GroupName, GroupRole = groupRequest.RequestRole, UserEmail = groupRequest.UserName, GroupRoleClaims = groupRequest.RequestRole == "Lead" ? ["Edit"] : [],ModifiedBy = groupRequest.ModifiedBy,ModifiedOnDt = groupRequest.ModifiedOnDt };

                await _groupUserProvider.UpdateGroupUser(newGroupGroupRequest);
                
                _decimDevContext.GroupRequests.Update(groupRequest);

                await _decimDevContext.SaveChangesAsync();

                return new ApiResponse<bool>(
                    ApiResponseType.Success,
                    true,
                    "Assigned user group role and updated status."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error occurred while updating request status.");
                throw;
            }
        }

        public async Task<ApiResponse<bool>> AddUserRequests(GroupRequest groupRequest)
        {
            try
            {
               var isRequestExist = await _decimDevContext.GroupRequests.Where(gr => gr.PracticeName == groupRequest.PracticeName && gr.GroupName == groupRequest.GroupName && gr.UserName == groupRequest.UserName).FirstOrDefaultAsync();

                if(isRequestExist != null)
                {
                    return  new ApiResponse<bool>(
                    ApiResponseType.Failure,
                    false,
                    "Request already exist."
                );
                }

                await _decimDevContext.GroupRequests.AddAsync(groupRequest);
                await _decimDevContext.SaveChangesAsync();

                return new ApiResponse<bool>(
                    ApiResponseType.Success,
                    true,
                    ""
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error occurred while updating request status.");
                throw;
            }
        }

        public async Task<ApiResponse<bool>> DeleteGroupRequest(List<int> groupRequestIds, IUserContext userContext)
        {
            try
            {
                var groupRequests = await _decimDevContext.GroupRequests
                                            .Where(gr => groupRequestIds.Contains(gr.Id))
                                            .ToListAsync();

                if (!userContext.Roles.Contains("Admin"))
                {
                    var groupNames = groupRequests.Select(g => g.GroupName).ToList();

                    bool isLead = await _decimDevContext.GroupUsers
                        .AnyAsync(gu => gu.UserEmail == userContext.Email && gu.GroupRole == "Lead" && groupNames.Contains(gu.Group));

                    if (!isLead)
                    {
                        throw new UnauthorizedAccessException("User is not a lead of all requested groups.");
                    }
                }

                _decimDevContext.GroupRequests.RemoveRange(groupRequests);

                await _decimDevContext.SaveChangesAsync();

                return new ApiResponse<bool>(
                    ApiResponseType.Success,
                    true,
                    "Group requests deleted successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error occurred while deleting group request");
                throw;
            }
        }
    }
}
