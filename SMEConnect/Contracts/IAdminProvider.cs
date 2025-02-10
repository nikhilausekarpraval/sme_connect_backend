using SMEConnect.Dtos;
using SMEConnect.Modals;

namespace SMEConnect.Contracts
{
    public interface IAdminProvider
    {

        public Task<string> AddRole(RoleDto role, string modifiyedBy);

        public Task<ApiResponse<string>> DeleteRoles(List<string> roleIds);

        public Task<string> AddRoleToUser(AssignRoleDto role);

        public Task<List<RoleWithClaimsDto>> GetRolesWithClaims();

        public Task<string> AddClaimToUser(AssignClaimDto userClaim);


        public Task<List<ApplicationRole>> GetRoles();


        public Task<List<ApplicationUser>> GetUsers();

        public Task<ApiResponse<string>> DeleteUser(List<string> users);

    }
}
