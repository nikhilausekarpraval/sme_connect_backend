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
    public class UserClaimProvider : IUserClaimProvider
    {
        private DcimDevContext _decimDevContext;
        private ILogger<UserClaimProvider> _logger;
        private UserManager<ApplicationUser> _userManager;

        public UserClaimProvider(DcimDevContext dcimDevContext, UserManager<ApplicationUser> userManager, ILogger<UserClaimProvider> logger)
        {
            this._decimDevContext = dcimDevContext;
            _logger = logger;
            _userManager = userManager;

        }

        public async Task<List<IdentityUserClaim<string>>> GetUserClaims()
        {
            try
            {
                var result = await _decimDevContext.UserClaims.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }
        }

        public async Task<ApiResponse<string>> DeleteUserClaims(List<int> userClaimIds)
        {
            try
            {
                using var transaction = await _decimDevContext.Database.BeginTransactionAsync();

                var userClaims = _decimDevContext.UserClaims.Where(rc => userClaimIds.Contains(rc.Id));

                _decimDevContext.UserClaims.RemoveRange(userClaims);

                await _decimDevContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<string>(
                    ApiResponseType.Success,
                    "",
                    "Users Claims deleted successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error occurred while deleting user claims");
                throw;
            }
        }

        public async Task<ApiResponse<string>> UpdateUserClaims(List<UserClaimDto> userClaimDtos)
        {
            try
            {
                using var transaction = await _decimDevContext.Database.BeginTransactionAsync();

                var userClaimIds = userClaimDtos.Select(rc => rc.Id).ToList();

                var existingUserClaims = await _decimDevContext.UserClaims
                    .Where(rc => userClaimIds.Contains(rc.Id))
                    .ToListAsync();

                foreach (var existingUserClaim in existingUserClaims)
                {
                    var userClaimDto = userClaimDtos.FirstOrDefault(rc => rc.Id == existingUserClaim.Id);

                    if (userClaimDto != null)
                    {
                        existingUserClaim.ClaimType = userClaimDto.ClaimType;
                        existingUserClaim.ClaimValue = userClaimDto.ClaimValue;
                    }
                }

                await _decimDevContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<string>(
                    ApiResponseType.Success,
                    "",
                    "User claims updated successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Error occurred while updating user claims.");
                throw;
            }
        }

        public async Task<string> AddClaimToUser(List<AddClaimToUserDto> userClaims)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userClaims[0].userId);
                if (user == null)
                {
                    return AccessConfigurationErrorMessage.UserNotFound;
                }

                foreach (var userClaim in userClaims)
                {

                    Claim claim = new Claim(userClaim.claimType, userClaim.claimValue);

                    var existingClaims = await _userManager.GetClaimsAsync(user);

                    var existingClaim = existingClaims.FirstOrDefault(c => c.Type == userClaim.claimType);

                    if (existingClaim != null)
                    {
                        var removeResult = await _userManager.RemoveClaimAsync(user, existingClaim);
                        if (!removeResult.Succeeded)
                        {
                            throw new Exception("Failed to remove existing claim.");
                        }
                    }

                    var result = await _userManager.AddClaimAsync(user, claim);
                }

                var userResult = await _userManager.UpdateAsync(user);

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
