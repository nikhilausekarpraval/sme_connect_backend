using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Modals;
using Microsoft.EntityFrameworkCore;
using static DemoDotNetCoreApplication.Constatns.Constants;


namespace DemoDotNetCoreApplication.Providers
{
    public class GroupUserProvider : IGroupUserProvider
    {
        private ILogger<GroupUserProvider> _logger;
        private DcimDevContext _dcimDevContext;
        private IUserContext _userContext;


        public GroupUserProvider(ILogger<GroupUserProvider> Logger, DcimDevContext dcimDevContext, IUserContext userContext)
        {
            this._dcimDevContext = dcimDevContext;
            this._logger = Logger;
            _userContext = userContext;
        }


        public async Task<ApiResponse<bool>> AddGroupUser(GroupUser group)
        {
            try
            {
                bool exists = await _dcimDevContext.GroupUsers.AnyAsync(p => p.UserEmail == group.UserEmail && p.Group == group.Group);
                if (exists)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "A user with the same group already exists.");
                }

                await _dcimDevContext.GroupUsers.AddAsync(group);
                await _dcimDevContext.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<List<GroupUser>>> getUserGroups()
        {
            try
            {   var user = _userContext.Email;
                List<GroupUser> roles = await _dcimDevContext.GroupUsers.Where(g => g.UserEmail == user).ToListAsync();
                await _dcimDevContext.SaveChangesAsync();
                return new ApiResponse<List<GroupUser>>(ApiResponseType.Success, roles);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
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

        public async Task<ApiResponse<bool>> UpdateGroupUser(GroupUser group)
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
                //existingGroup.ModifiedBy = _userContext.Email;
                //existingGroup.ModifiedDate = DateTime.UtcNow;

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

