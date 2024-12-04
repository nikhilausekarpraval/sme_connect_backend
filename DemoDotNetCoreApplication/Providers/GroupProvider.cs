using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static DemoDotNetCoreApplication.Constatns.Constants;

namespace DemoDotNetCoreApplication.Providers
{
    public class GroupProvider
    {
        private ILogger<GroupProvider> _logger;
        private DcimDevContext _decimDevContext;


        public GroupProvider(ILogger<GroupProvider> Logger, DcimDevContext decimDevContext)
        {
            this._decimDevContext = decimDevContext;
            this._logger = Logger;
        }

        public async Task<string> AddGroup(RoleDto role)
        {

            try
            {
                //var roleExist = await _roleManager.RoleExistsAsync(role.roleName);
                //if (!roleExist)
                //{
                //   // await _roleManager.CreateAsync(new IdentityRole(role.roleName));
                //    return AccessConfigurationSccessMessage.NewRoleAdded;
                //}
                //else
                //{
                //    return AccessConfigurationErrorMessage.RoleExist;
                //}
                return "";
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return AccessConfigurationErrorMessage.ErrorWhileCreatingRole;
            }

        }

        //public async Task<List<IdentityRole<string>> getGroups()
        //{
        //    try
        //    {
        //        var roles = await _roleManager.Roles.ToListAsync();
        //        return roles;
        //    }
        //    catch (Exception ex)
        //    {
        //        this._logger.LogError(1, ex, ex.Message);
        //        throw;
        //    }
        //}


        //public async Task<ApiResponse<string> DeleteGroup(RoleDto role)
        //{
        //    try
        //    {
        //        var userRoles = _decimDevContext.UserRoles.Where(ur => userIds.Contains(ur.UserId));
        //        var userClaims = _decimDevContext.UserClaims.Where(uc => userIds.Contains(uc.UserId));
        //        var userLogins = _decimDevContext.UserLogins.Where(ul => userIds.Contains(ul.UserId));

        //        _decimDevContext.UserRoles.RemoveRange(userRoles);
        //        _decimDevContext.UserClaims.RemoveRange(userClaims);
        //        _decimDevContext.UserLogins.RemoveRange(userLogins);

        //        var usersToDelete = _decimDevContext.Users.Where(u => userIds.Contains(u.Id));
        //        _decimDevContext.Users.RemoveRange(usersToDelete);

        //        await _decimDevContext.SaveChangesAsync();

        //        return new ApiResponse<string>(
        //            ApiResponseType.Success,
        //            "",
        //            "Users deleted successfully."
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return new ApiResponse<string>(
        //            ApiResponseType.Failure,
        //            "",
        //            $"An error occurred while deleting users: {ex.Message}"
        //        );
        //    }
        //}

    }

}

