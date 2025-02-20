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

        public GroupRequestsPorvider(DcimDevContext dcimDevContext, UserManager<ApplicationUser> userManager, ILogger<UserClaimProvider> logger)
        {
            this._decimDevContext = dcimDevContext;
            _logger = logger;
            _userManager = userManager;

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
                    return new ApiResponse<List<GroupRequest>>(ApiResponseType.Success, groupRequests,"");
                
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


        public async Task<ApiResponse<bool>> UpdateUserRequests(GroupRequest groupRequest )
        {
            try
            {
                _decimDevContext.GroupRequests.Update(groupRequest);

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

        public async Task<ApiResponse<bool>> AddUserRequests(GroupRequest groupRequest)
        {
            try
            {
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

        public async Task<ApiResponse<bool>> DelereGroupRequest(List<int> groupRequestIds)
        {
            try
            {
                using var transaction = await _decimDevContext.Database.BeginTransactionAsync();

                var groupRequests = await _decimDevContext.GroupRequests.Where(gr => groupRequestIds.Contains(gr.Id)).ToListAsync();

                _decimDevContext.GroupRequests.RemoveRange(groupRequests);

                await _decimDevContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ApiResponse<bool>(
                    ApiResponseType.Success,
                    true,
                    ""
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
