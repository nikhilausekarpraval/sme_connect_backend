using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Dtos;

namespace SMEConnect.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin")]
    [Authorize(AuthenticationSchemes = "CustomJwt, AzureAD")]
    [Route("api/[controller]")]
    public class UserClaimsController : ControllerBase
    {

        private IUserClaimProvider _UserClaimProvider;


        public UserClaimsController(IUserClaimProvider UserClaimProvider)
        {
            this._UserClaimProvider = UserClaimProvider;
        }


        [HttpPost]
        [Route("add_claim_to_user")]
        public async Task<IActionResult> AddUserClaim([FromBody] List<AddClaimToUserDto> UserClaim)
        {

            try
            {
                var result = await this._UserClaimProvider.AddClaimToUser(UserClaim);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }

        }
        [HttpGet]
        [Route("get_user_claims")]
        public async Task<IActionResult> GetUserClaims()
        {
            try
            {
                var result = await this._UserClaimProvider.GetUserClaims();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpPut("update_user_claim")]
        public async Task<ActionResult> UpdataeUserClaim([FromBody] List<UserClaimDto> employeeDto)
        {
            try
            {
                var response = await _UserClaimProvider.UpdateUserClaims(employeeDto);

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
        [Route("delete_user_claim")]
        public async Task<IActionResult> DeleteUserClaim([FromBody] List<int> UserClaimIds)
        {
            try
            {
                var result = await this._UserClaimProvider.DeleteUserClaims(UserClaimIds);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

    }
}


