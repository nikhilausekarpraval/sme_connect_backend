using Microsoft.AspNetCore.Identity;
using SMEConnect.Dtos;
using SMEConnect.Modals;

namespace SMEConnect.Contracts
{
    public interface IRoleClaimProvider
    {
        public Task<List<IdentityRoleClaim<string>>> GetRoleClaims();


        public Task<ApiResponse<string>> DeleteRoleClaims(List<string> roleClaimIds);


        public Task<ApiResponse<string>> UpdateRoleClaims(List<RoleClaimDto> roleClaimDtos);


        public Task<string> AddClaimToRole(AddClaimToRoleDto roleClaims);

        public Task<List<RoleClaimWithRolesDto>> GetRoleClaimsWithRolesAsync();

    }
}
