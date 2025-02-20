using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Contracts;
using SMEConnect.Dtos;
using SMEConnect.Modals;

namespace SMEConnect.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "CustomJwt, AzureAD")]
    [Route("api/[controller]")]
    public class GroupUserRoleClaimController : ControllerBase
    {

        private IGroupUserRoleClaimProvider _groupUserRoleClaimProvider;
        public GroupUserRoleClaimController(IGroupUserRoleClaimProvider groupUserRoleClaimProvider) 
        {
            _groupUserRoleClaimProvider = groupUserRoleClaimProvider;
        }

        [HttpPost]
        [Route("add_group_role_claims")]
        public async Task<IActionResult> GetGroupUsers([FromBody] List<GroupUserRoleClaim> groupUserRoleClaims)
        {
            try
            {
                var result = await _groupUserRoleClaimProvider.CreateUpdateGroupClaim(groupUserRoleClaims);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }

        [HttpPost]
        [Route("get_group_user_role_claims")]
        public async Task<IActionResult> GetGroupUserRoleClaims([FromBody] RoleIdDto roleId)
        {
            try
            {
                var result = await this._groupUserRoleClaimProvider.GetGroupUserRoles(roleId.RoleId);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }
    }
}
