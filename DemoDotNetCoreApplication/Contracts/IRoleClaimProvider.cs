using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface IRoleClaimProvider
    {
        public  Task<List<IdentityRoleClaim<string>>> GetRoleClaims();


        public  Task<ApiResponse<string>> DeleteRoleClaims(List<string> roleClaimIds);


        public  Task<ApiResponse<string>> UpdateRoleClaims(List<RoleClaimDto> roleClaimDtos);


        public  Task<string> AddClaimToRole(List<AddClaimToRoleDto> roleClaims);

        public Task<List<RoleClaimWithRolesDto>> GetRoleClaimsWithRolesAsync();

    }
}
