﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Dtos;
using SMEConnect.Modals;
using System.Security.Claims;
using static SMEConnect.Constatns.Constants;


namespace SMEConnect.Providers
{
    public class AdminProvider : IAdminProvider
    {

        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationRole> _roleManager;
        private ILogger<AdminProvider> _logger;
        private DcimDevContext _decimDevContext;
        private IUserContext _userContext;


        public AdminProvider(IServiceProvider serviceProvider, ILogger<AdminProvider> Logger, DcimDevContext decimDevContext, IUserContext userContext)
        {
            this._userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            this._roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            this._decimDevContext = decimDevContext;
            this._userContext = userContext;
            this._logger = Logger;
        }

        public async Task<string> AddRole(RoleDto role)
        {

            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(role.Name);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = role.Name, ModifiedBy = _userContext.Email });
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

        public async Task<string> AddRoleToUser(AssignRoleDto role)
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
                }
                else
                {
                    return AccessConfigurationErrorMessage.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return AccessConfigurationErrorMessage.FailedToAddRoleToUser;
            }

        }

        public async Task<string> AddClaimToUser(AssignClaimDto userClaim)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userClaim.userId);
                if (user != null)
                {
                    var claim = new Claim(userClaim.claimType, userClaim.claimValue);
                    var result = await _userManager.AddClaimAsync(user, claim);

                    return result.Succeeded ? AccessConfigurationSccessMessage.ClaimAddedToUser : AccessConfigurationErrorMessage.FailedToAddClaimToUser;

                }
                else
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

        public async Task<List<ApplicationRole>> GetRoles()
        {
            try
            {
                var roles = await _decimDevContext.Roles.ToListAsync();
                return roles;
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

        public async Task<List<RoleWithClaimsDto>> GetRolesWithClaims()
        {
            try
            {

                var rolesWithClaims = await _decimDevContext.Roles
                    .Select(role => new RoleWithClaimsDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        ModifiedBy = role.ModifiedBy,
                        ModifiedOnDt = role.ModifiedOnDt,
                        Claims = _decimDevContext.RoleClaims
                            .Where(rc => rc.RoleId == role.Id)
                            .Select(rc => new IdentityRoleClaim<string>
                            {
                                Id = rc.Id,
                                RoleId = rc.RoleId,
                                ClaimType = rc.ClaimType,
                                ClaimValue = rc.ClaimValue
                            }).ToList()
                    })
                    .ToListAsync();

                return rolesWithClaims;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching roles with claims.");
                throw;
            }
        }

        public async Task<ApiResponse<string>> DeleteRoles(List<string> roleIds)
        {
            try
            {
                using var transaction = await _decimDevContext.Database.BeginTransactionAsync();

                var userRoles = _decimDevContext.UserRoles.Where(ur => roleIds.Contains(ur.RoleId));
                var roleClaims = _decimDevContext.RoleClaims.Where(rc => roleIds.Contains(rc.RoleId));

                _decimDevContext.UserRoles.RemoveRange(userRoles);
                _decimDevContext.RoleClaims.RemoveRange(roleClaims);

                var rolesToDelete = _decimDevContext.Roles.Where(r => roleIds.Contains(r.Id));
                _decimDevContext.Roles.RemoveRange(rolesToDelete);

                await _decimDevContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<string>(
                    ApiResponseType.Success,
                    "",
                    "Roles deleted successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting roles. Role IDs: {RoleIds}", string.Join(", ", roleIds));
                throw;
            }
        }


        public async Task<List<ApplicationUser>> GetUsers()
        {
            try
            {
                var query = from user in _userManager.Users
                            join userRole in _decimDevContext.UserRoles on user.Id equals userRole.UserId
                            join role in _decimDevContext.Roles on userRole.RoleId equals role.Id
                            join userClaims in _decimDevContext.UserClaims on user.Id equals userClaims.UserId into userClaimsGroup
                            from userClaims in userClaimsGroup.DefaultIfEmpty()
                            select new { user, role, userClaims };

                var userRolesData = await query.ToListAsync();


                var userWithRoles = userRolesData
                                    .GroupBy(x => x.user.Id)
                                    .Select(g =>
                                    {
                                        var user = g.First().user;
                                        return new ApplicationUser
                                        {
                                            Id = user.Id,
                                            UserName = user.UserName,
                                            DisplayName = user.DisplayName,
                                            Email = user.Email,
                                            ModifiedOnDt = user.ModifiedOnDt,
                                            ModifiedBy = user.ModifiedBy,
                                            PhoneNumber = user.PhoneNumber,
                                            Practice = user.Practice,
                                            Claims = g
                                            .Where(x => x.userClaims != null)
                                            .GroupBy(c => new { c.userClaims.ClaimType, c.userClaims.ClaimValue })
                                            .Select(x => new UserClaimDto
                                            {
                                                Id = x.First().userClaims.Id,
                                                UserId = x.First().userClaims.UserId,
                                                ClaimType = x.Key.ClaimType,
                                                ClaimValue = x.Key.ClaimValue,
                                            }).ToList(),
                                            Roles = g
                                            .GroupBy(r => r.role.Id)
                                            .Select(x => new RoleDto
                                            {
                                                Id = x.Key,
                                                Name = x.First().role.Name
                                            }).ToList()
                                        };
                                    }).ToList();

                return userWithRoles;

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }


        public async Task<List<IdentityUserClaim<string>>> GetUserClaims()
        {
            try
            {
                var claims = await _decimDevContext.UserClaims.ToListAsync();
                return claims;
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }

        }

        public async Task<ApiResponse<string>> DeleteUser(List<string> userIds)
        {
            try
            {
                var userRoles = _decimDevContext.UserRoles.Where(ur => userIds.Contains(ur.UserId));
                var userClaims = _decimDevContext.UserClaims.Where(uc => userIds.Contains(uc.UserId));
                var userLogins = _decimDevContext.UserLogins.Where(ul => userIds.Contains(ul.UserId));

                _decimDevContext.UserRoles.RemoveRange(userRoles);
                _decimDevContext.UserClaims.RemoveRange(userClaims);
                _decimDevContext.UserLogins.RemoveRange(userLogins);

                var usersToDelete = _decimDevContext.Users.Where(u => userIds.Contains(u.Id));
                _decimDevContext.Users.RemoveRange(usersToDelete);

                await _decimDevContext.SaveChangesAsync();

                return new ApiResponse<string>(
                    ApiResponseType.Success,
                    "",
                    "Users deleted successfully."
                );

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ApiResponse<string>(
                    ApiResponseType.Failure,
                    "",
                    $"An error occurred while deleting users: {ex.Message}"
                );
            }
        }


    }

}

