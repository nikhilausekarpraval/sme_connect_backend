using SMEConnect.Modals;

namespace SMEConnect.Contracts
{
    public interface IGroupUserRoleClaimProvider
    {
        public Task<List<GroupUserRoleClaim>> GetGroupUserRoles(int roleId);
        public Task<bool> CreateUpdateGroupClaim(List<GroupUserRoleClaim> groupUserRoleClaim);

    }
}
