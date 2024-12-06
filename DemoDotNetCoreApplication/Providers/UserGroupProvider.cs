using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static DemoDotNetCoreApplication.Constatns.Constants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DemoDotNetCoreApplication.Providers
{
    public class UserGroupProvider : IGroupProvider
    {
        private ILogger<UserGroupProvider> _logger;
        private DcimDevContext _dcimDevContext;


        public UserGroupProvider(ILogger<UserGroupProvider> Logger, DcimDevContext dcimDevContext)
        {
            this._dcimDevContext = dcimDevContext;
            this._logger = Logger;
        }

        public async Task<string> AddGroup(UserGroup userGroup)
        {

            try
            {
                var result = await _dcimDevContext.UserGroups.AddAsync(userGroup);
                await _dcimDevContext.SaveChangesAsync();
                return AccessConfigurationSccessMessage.AddedNewGroup;
                

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }

        }

        public async Task<ApiResponse<List<UserGroup>>> getGroups()
        {
            try
            {
                List<UserGroup> roles = await _dcimDevContext.UserGroups.ToListAsync();
                await _dcimDevContext.SaveChangesAsync();
                return new ApiResponse<List<UserGroup>> (ApiResponseType.Success, roles);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }


        public async Task<ApiResponse<bool>> DeleteUserGroup(List<UserGroup> ids)
        {
            try
            {
                    _dcimDevContext.UserGroups.RemoveRange(ids);
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

