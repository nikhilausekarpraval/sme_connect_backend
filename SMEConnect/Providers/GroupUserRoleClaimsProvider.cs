using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MongoDB.Driver;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Modals;

namespace SMEConnect.Providers
{
    public class GroupUserRoleClaimsProvider : IGroupUserRoleClaimProvider
    {
        private DcimDevContext _dcimDevContext;

        public GroupUserRoleClaimsProvider(DcimDevContext dcimDevContext)
        {
            _dcimDevContext = dcimDevContext;
        }

        public async Task<bool> CreateUpdateGroupClaim(List<GroupUserRoleClaim> groupUserRoleClaim)
        {
            try
            {
                await _dcimDevContext.GroupUserRoleClaims
                   .Where(c => c.GroupUserId == groupUserRoleClaim[0].GroupUserId).ExecuteDeleteAsync();

                await _dcimDevContext.GroupUserRoleClaims.AddRangeAsync(groupUserRoleClaim);

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<GroupUserRoleClaim>> GetGroupUserRoles(int roleId)
        {
            try
            {
                var result = await _dcimDevContext.GroupUserRoleClaims.Where(g => g.GroupUserId == roleId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

