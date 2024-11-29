using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using static DemoDotNetCoreApplication.Constatns.Constants;
using DemoDotNetCoreApplication.Contracts;


namespace DemoDotNetCoreApplication.Providers
{
    public class AdminProvider : IAdminProvider
    {

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ILogger<AdminProvider> _logger;

        public AdminProvider(IServiceProvider serviceProvider, ILogger<AdminProvider> Logger)
        {
            this._userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            this._roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            this._logger = Logger;
        }

        public async Task<string> AddRole( RoleDto role)
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
                this._logger.LogError(1,ex,ex.Message);
                return AccessConfigurationErrorMessage.ErrorWhileCreatingRole;
            }

        }

        public async Task<string> AddRoleToUser( AssignRoleDto role)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(role.userId);
                if (user != null)
                {
                    var result = await _userManager.AddToRoleAsync(user, role.roleName);
                    if (result.Succeeded)
                    {
                        return AccessConfigurationSccessMessage.AddedRoleToUser;
                    }
                    else
                    {
                       return result.Errors.First().Description;
                    }
                }else
                {
                    return AccessConfigurationErrorMessage.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(1,ex, ex.Message);
                return AccessConfigurationErrorMessage.FailedToAddRoleToUser;
            }

        }

        public async Task<string> AddClaimToUser( AssignClaimDto userClaim)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userClaim.userId);
                if (user != null)
                {
                    var claim = new Claim(userClaim.claimType, userClaim.claimValue);
                    var result = await _userManager.AddClaimAsync(user, claim);

                    return result.Succeeded ? AccessConfigurationSccessMessage.ClaimAddedToUser : AccessConfigurationErrorMessage.FailedToAddClaimToUser;
                    
                }else
                {
                    return AccessConfigurationErrorMessage.UserNotFound;
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return AccessConfigurationErrorMessage.ErrorWhileAddingClaim;
            }

        }

        public async Task<string> AddClaimToRole( AddClaimToRoleDto roleClaim)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleClaim.roleName);
                if (role == null)
                {
                    return AccessConfigurationErrorMessage.RoleNotFound;
                }

                Claim claim = new Claim(roleClaim.claimType, roleClaim.claimValue);
                var result = await _roleManager.AddClaimAsync(role, claim);

                 return result.Succeeded ? AccessConfigurationSccessMessage.ClaimAddedToUser : AccessConfigurationErrorMessage.ErrorWhileAddingClaim;
                
            }
            catch (Exception ex) 
            {
                this._logger.LogError(1, ex, ex.Message);
                return AccessConfigurationErrorMessage.ErrorWhileAddingClaim;
            }

        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                return roles;
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

        public async Task<List<ApplicationUser>> GetUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }


    }

}

