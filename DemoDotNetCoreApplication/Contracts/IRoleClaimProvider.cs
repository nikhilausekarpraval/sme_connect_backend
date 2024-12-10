using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using static DemoDotNetCoreApplication.Constatns.Constants;
using System.Security.Claims;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface IRoleClaimProvider
    {
        public  Task<List<IdentityRoleClaim<string>>> GetRoleClaims();


        public  Task<ApiResponse<string>> DeleteRoleClaims(List<int> roleClaimIds);


        public  Task<ApiResponse<string>> UpdateRoleClaims(List<RoleClaimDto> roleClaimDtos);


        public  Task<string> AddClaimToRole(List<AddClaimToRoleDto> roleClaims);


    }
}
