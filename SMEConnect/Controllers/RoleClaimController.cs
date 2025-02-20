using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Constatns;
using SMEConnect.Dtos;
using SMEConnect.Modals;
using SMEConnect.Providers;

namespace SMEConnect.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = "CustomJwt, AzureAD")]
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
        public async Task<IActionResult> AddRoleClaim([FromBody] AddClaimToRoleDto RoleClaim)
        {

            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                if (!userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                var result = await this._roleClaimProvider.AddClaimToRole(RoleClaim);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }

        }
        [HttpGet]
        [Route("get_role_claims")]
        public async Task<IActionResult> GetRoleClaims()
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                if (!userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                var result = await this._roleClaimProvider.GetRoleClaims();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }

        [HttpGet]
        [Route("get_claims_roles")]
        public async Task<IActionResult> GetClaimAndRoles()
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                if (!userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                var result = await this._roleClaimProvider.GetRoleClaimsWithRolesAsync();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }

        [HttpPut("update_role_claim")]
        public async Task<ActionResult> UpdataeRoleClaim([FromBody] List<RoleClaimDto> employeeDto)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                if (!userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
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
                return new JsonResult(NotFound(ex.Message));
            }

        }

        [HttpDelete]
        [Route("delete_role_claim")]
        public async Task<IActionResult> DeleteRoleClaim([FromBody] List<string> RoleClaimIds)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                if (!userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                var result = await this._roleClaimProvider.DeleteRoleClaims(RoleClaimIds);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }

    }
}
