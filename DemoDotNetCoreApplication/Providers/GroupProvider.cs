using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
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

        public async Task<string> AddRole(RoleDto role)
        {

            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(role.roleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.roleName));
                    return AccessConfigurationSccessMessage.NewRoleAdded;
                }
                else
                {
                    return AccessConfigurationErrorMessage.RoleExist;
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return AccessConfigurationErrorMessage.ErrorWhileCreatingRole;
            }

        }
    }
}
