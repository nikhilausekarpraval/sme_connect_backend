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
        public class UserClaimsController : ControllerBase
        {

            private UserClaimProvider _UserClaimProvider;


            public UserClaimsController(UserClaimProvider UserClaimProvider)
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
            public async Task<IActionResult> DeleteUserClaim([FromBody] List<string> UserClaimIds)
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


