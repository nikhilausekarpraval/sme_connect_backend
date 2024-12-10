using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoDotNetCoreApplication.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class RoleClaimController : ControllerBase
    {

        private RoleClaimProvider _roleClaimProvider;


        public RoleClaimController(RoleClaimProvider roleClaimProvider)
        {
            this._roleClaimProvider = roleClaimProvider;
        }


        [HttpPost]
        [Route("add_claim_to_role")]
        public async Task<IActionResult> AddRoleClaim([FromBody] List<AddClaimToRoleDto> RoleClaim)
        {

            try
            {
                var result = await this._roleClaimProvider.AddClaimToRole(RoleClaim);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }

        }
        [HttpGet]
        [Route("get_role_claims")]
        public async Task<IActionResult> GetRoleClaims()
        {
            try
            {
                var result = await this._roleClaimProvider.GetRoleClaims();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpGet]
        [Route("get_claims_roles")]
        public async Task<IActionResult> GetClaimAndRoles()
        {
            try
            {
                var result = await this._roleClaimProvider.GetRoleClaimsWithRolesAsync();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpPut("update_role_claim")]
        public async Task<ActionResult> UpdataeRoleClaim([FromBody] List<RoleClaimDto> employeeDto)
        {
            try
            {
                var response = await _roleClaimProvider.UpdateRoleClaims(employeeDto);

                if (response.Status == Constants.ApiResponseType.Success)
                {
                    return new JsonResult(Ok(response)); 
                }
                else
                {
                    return new JsonResult(StatusCode(500, response.Message)); 
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }

        }

        [HttpDelete]
        [Route("delete_role_claim")]
        public async Task<IActionResult> DeleteRoleClaim([FromBody] List<int> RoleClaimIds)
        {
            try
            {
                var result = await this._roleClaimProvider.DeleteRoleClaims(RoleClaimIds);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

    }
}
