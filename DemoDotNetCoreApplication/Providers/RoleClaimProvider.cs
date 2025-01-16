using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static DemoDotNetCoreApplication.Constatns.Constants;
using System.Security.Claims;

namespace DemoDotNetCoreApplication.Providers
{
    public class RoleClaimProvider : IRoleClaimProvider
    {
        private DcimDevContext _decimDevContext;
        private ILogger<RoleClaimProvider> _logger;
        private RoleManager<ApplicationRole> _roleManager;

        public RoleClaimProvider(DcimDevContext dcimDevContext,RoleManager<ApplicationRole> roleManager, ILogger<RoleClaimProvider> logger) {
         this._decimDevContext = dcimDevContext;
            _logger = logger;
            _roleManager = roleManager;
        
        }

        public async Task<List<IdentityRoleClaim<string>>> GetRoleClaims()
        {
            try
            {
                var result = await _decimDevContext.RoleClaims.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }
        }

        public async Task<List<RoleClaimWithRolesDto>> GetRoleClaimsWithRolesAsync()
        {
            try
            {
            var roleClaimsWithRoles = await (from roleClaim in _decimDevContext.RoleClaims
                                                join role in _decimDevContext.Roles on roleClaim.RoleId equals role.Id
                                                group new { roleClaim, role } by new { roleClaim.ClaimType, roleClaim.ClaimValue } into grouped
                                                select new RoleClaimWithRolesDto
                                                {
                                                    id = grouped.First().roleClaim.Id, 
                                                    claimType = grouped.Key.ClaimType,
                                                    claimValue = grouped.Key.ClaimValue,
                                                    roles = grouped.Select(g => new RoleDto
                                                    {
                                                        Id = g.role.Id,
                                                        Name = g.role.Name
                                                    }).Distinct().ToList() 
                                                }).ToListAsync();

                return roleClaimsWithRoles;
            }
            catch (Exception ex)
            {
                _logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }
        }

        public async Task<ApiResponse<string>> DeleteRoleClaims(List<string> roleClaimIds)
        {
            try
            {
                using var transaction = await _decimDevContext.Database.BeginTransactionAsync();

                var roleClaims = _decimDevContext.RoleClaims.Where(rc => roleClaimIds.Contains(rc.RoleId));

                _decimDevContext.RoleClaims.RemoveRange(roleClaims);

                await _decimDevContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<string>(
                    ApiResponseType.Success,
                    "",
                    "Roles Claims deleted successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error occurred while deleting role claims");
                throw;
            }
        }

        public async Task<ApiResponse<string>> UpdateRoleClaims(List<RoleClaimDto> roleClaimDtos)
        {
            try
            {
                using var transaction = await _decimDevContext.Database.BeginTransactionAsync();

                var roleClaimIds = roleClaimDtos.Select(rc => rc.Id).ToList();

                var existingRoleClaims = await _decimDevContext.RoleClaims
                    .Where(rc => roleClaimIds.Contains(rc.Id))
                    .ToListAsync();

                foreach (var existingRoleClaim in existingRoleClaims)
                {
                    var roleClaimDto = roleClaimDtos.FirstOrDefault(rc => rc.Id == existingRoleClaim.Id);

                    if (roleClaimDto != null)
                    {
                        existingRoleClaim.ClaimType = roleClaimDto.ClaimType;
                        existingRoleClaim.ClaimValue = roleClaimDto.ClaimValue;
                    }
                }

                await _decimDevContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<string>(
                    ApiResponseType.Success,
                    "",
                    "Role claims updated successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error occurred while updating role claims.");
                throw;
            }
        }



        public async Task<string> AddClaimToRole(AddClaimToRoleDto roleClaim)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleClaim?.roleId);
                if (role == null)
                {
                    return AccessConfigurationErrorMessage.RoleNotFound;
                }

                    Claim claim = new Claim(roleClaim.claimType, roleClaim.claimValue);

                    var existingClaims = await _roleManager.GetClaimsAsync(role);

                    var existingClaim = existingClaims.FirstOrDefault(c => c.Type == roleClaim.claimType);

                    if (existingClaim != null)
                    {
                        var removeResult = await _roleManager.RemoveClaimAsync(role, existingClaim);
                        if (!removeResult.Succeeded)
                        {
                            throw new Exception("Failed to remove existing claim.");
                        }
                    }

                    var result = await _roleManager.AddClaimAsync(role, claim);

                var roleResult = await _roleManager.UpdateAsync(role);

                return AccessConfigurationSccessMessage.ClaimAddedToUser;

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }

        }
    }
}
