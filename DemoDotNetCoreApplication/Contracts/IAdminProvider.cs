using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using static DemoDotNetCoreApplication.Constatns.Constants;
using System.Security.Claims;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface IAdminProvider
    {
        public  Task<string> AddRole(RoleDto role);

        public Task<ApiResponse<string>> DeleteRoles(List<string> roleIds);

        public  Task<string> AddRoleToUser(AssignRoleDto role);

        public Task<List<RoleWithClaimsDto>> GetRolesWithClaims();

        public  Task<string> AddClaimToUser(AssignClaimDto userClaim);


        public  Task<string> AddClaimToRole(List<AddClaimToRoleDto> roleClaim);


        public  Task<List<ApplicationRole>> GetRoles();


        public  Task<List<ApplicationUser>> GetUsers();

        public Task<ApiResponse<string>> DeleteUser(List<string> users);

    }
}
