namespace SMEConnect.Controllers
{
    using SMEConnect.Data;
    using SMEConnect.Dtos;
    using SMEConnect.Modals;
    using SMEConnect.Modals.JWTAuthentication.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading.Tasks;
    using SMEConnect.Helpers;
    using System.Linq;
    using SMEConnect.Contracts;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using static SMEConnect.Constatns.Constants;

    namespace JWTAuthentication.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthenticateController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly RoleManager<ApplicationRole> roleManager;
            private readonly IConfiguration _configuration;
            private readonly SignInManager<ApplicationUser> signInManager;
            private readonly DcimDevContext _dcimDevContext;
            private readonly IAuthenticationProvider _authenticationProvider;

            public AuthenticateController(UserManager<ApplicationUser> userManager, IAuthenticationProvider authenticationProvider, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, DcimDevContext dcimDevContext)
            {
                this.userManager = userManager;
                this.roleManager = roleManager;
                _configuration = configuration;
                this.signInManager = signInManager;
                _dcimDevContext = dcimDevContext;
                this._authenticationProvider = authenticationProvider;
            }



            [HttpPost]
            [Route("login")]
            public async Task<IActionResult> Login([FromBody] LoginModalDto model)
            {
                // var user = await userManager.FindByEmailAsync(model.UserName);
                try
                {
                  var result =  await this._authenticationProvider.Login(model);
                    return new JsonResult(result?.data != null ? Ok(result.data) : Unauthorized(result));
                }
                catch (Exception ex)
                {
                    return new JsonResult( NotFound(new ResponseDto { message = "Error", status = "", statusText = ex.Message }));
                }
                
            }

            //[HttpPost]
            //[Route("getUserContext")]
            //public async Task<IActionResult> GetUserContext ([FromBody] UserDto userDto)
            //{
            //    try
            //    {
            //        var user = await userManager.Users.ToListAsync();
            //        if (user == null)
            //        {
            //            return new JsonResult(NotFound("User not found"));
            //        }

            //        var role = await roleManager.Roles.ToListAsync();
            //        var userclaims = await userManager.GetClaimsAsync();
            //        var roleClaims = await roleManager.GetClaimsAsync(role);
            //    }
            //    catch (Exception ex)
            //    {

            //    }


            //}


            [HttpPost]
            [Route("register")]
            public async Task<IActionResult> Register([FromBody] RegisterModelDto model)
            {

                try
                {
                    var result = await this._authenticationProvider.Register(model);

                    return new JsonResult(result.statusText == ApiErrors.UserCreated ? Ok(result) : BadRequest(result));

                }
                catch (Exception ex)
                {
                    return new JsonResult(StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { status = "Error", message = ex.Message, statusText = ex.Message }));
                }

            }

            [HttpGet]
            [Route("logout")]
            public async Task<IActionResult> Logout()
            {
                try
                {

                    var result = await this._authenticationProvider.Logout();

                    return new JsonResult(Ok(result));
                }
                catch(Exception ex)
                {
                    return new JsonResult(NotFound(new ResponseDto { status = "Error", message = ex.Message, statusText = ex.Message })); 
                }

            }

            [HttpPut]
            [Authorize]
            [Route("update_user")]
            public async Task<IActionResult> UpdateUser([FromBody] RegisterModelDto user)
            {
                try
                {
                    var result = await this._authenticationProvider.UpdateUser(user);
                    return new JsonResult(result.statusText == AccessConfigurationSccessMessage.UpdatedUser ? Ok(result) : BadRequest(result));

                }
                catch (Exception ex)
                {
                    return new JsonResult(NotFound(new ResponseDto { status = "Error", message = "" ,statusText = ex.Message}));
                }

            }

            [HttpPut]
            [Authorize]
            [Route("reset_password")]
            public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto user)
            {
                try
                {

                    var result = await this._authenticationProvider.ResetPassword(user);
                    return new JsonResult(result.statusText == AccessConfigurationSccessMessage.PasswordUserUpdated ? Ok(result) : BadRequest(result));
                }
                catch (Exception ex)
                {
                    return new JsonResult(NotFound(new ResponseDto { statusText = ex.Message, status="Error", message="" }));
                }

            }


            [HttpPut]
            [Route("forget_password")]
            public async Task<IActionResult> ForgetPassword([FromBody] ResetPasswordDto user)
            {
                try
                {
                    var result = await this._authenticationProvider.ForgetPassword(user);
                    return new JsonResult(result.statusText == AccessConfigurationSccessMessage.PasswordUserUpdated ? Ok(result) : BadRequest(result));
                }
                catch (Exception ex)
                {
                    return new JsonResult(NotFound(ex.Message));
                }

            }

            [HttpPost]
            //[Authorize(Roles = "Admin")]
            //[Authorize(Policy ="ManageSystem")]
            [Route("register-admin")]
            public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModelDto model)
            {
                try
                {
                    var result = await this._authenticationProvider.RegisterAdmin(model);
                    return new JsonResult(result.statusText == AccessConfigurationSccessMessage.UserCreated ? Ok(result) : BadRequest(result));
                }
                catch (Exception ex) 
                {
                    return new JsonResult(NotFound(ex.Message));
                }

            }
        }
    }
}
