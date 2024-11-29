namespace DemoDotNetCoreApplication.Controllers
{
    using DemoDotNetCoreApplication.Data;
    using DemoDotNetCoreApplication.Dtos;
    using DemoDotNetCoreApplication.Modals;
    using DemoDotNetCoreApplication.Modals.JWTAuthentication.Authentication;
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
    using DemoDotNetCoreApplication.Helpers;
    using System.Linq;
    using DemoDotNetCoreApplication.Contracts;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using DemoDotNetCoreApplication.Constatns;
    using static DemoDotNetCoreApplication.Constatns.Constants;

    namespace JWTAuthentication.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AuthenticateController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly IConfiguration _configuration;
            private readonly SignInManager<ApplicationUser> signInManager;
            private readonly DcimDevContext _dcimDevContext;
            private readonly IAuthenticationProvider _authenticationProvider;

            public AuthenticateController(UserManager<ApplicationUser> userManager, IAuthenticationProvider authenticationProvider, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, DcimDevContext dcimDevContext)
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
                    return new JsonResult(result.data != null ? Ok(result.data) : Unauthorized(result));
                }
                catch (Exception ex)
                {
                    return new JsonResult( NotFound(new { message = "Error", status = 400, statusText = ex.Message }));
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

            [HttpPost]
            [Route("logout")]
            public async Task<IActionResult> Logout()
            {
                try
                {
                    await signInManager.SignOutAsync();

                    return new JsonResult(Ok(new ResponseDto { status = "Success", statusText = "User logged out successfully!", message = "" }));
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
                   var currentUser = await userManager.FindByEmailAsync(user.email);
                  if(currentUser != null &&  await userManager.CheckPasswordAsync(currentUser, user.password))
                    {
                        currentUser.DisplayName = user.displayName;
                        currentUser.UserName = user.userName;
                        currentUser.Email = user.email;
                        var result = await userManager.UpdateAsync(currentUser);
                        return new JsonResult(Ok(result));
                    }else
                    {
                        throw new Exception("User email or password is wrong");
                    }
                    
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

                    var currentUser = await userManager.FindByEmailAsync(user.email);

                    if (currentUser != null)
                    {

                            if (await userManager.CheckPasswordAsync(currentUser, user.password))
                            {
                               
                                var token = await userManager.GeneratePasswordResetTokenAsync(currentUser);
                                var passwordResetResult = await userManager.ResetPasswordAsync(currentUser, token, user.newPassword);

                                if (passwordResetResult.Succeeded)
                                {
                                    
                                    currentUser.DisplayName = user.displayName;
                                    currentUser.UserName = user.userName;
                                    currentUser.Email = user.email;

                                    var updateResult = await userManager.UpdateAsync(currentUser);

                                    if (updateResult.Succeeded)
                                    {
                                        return new JsonResult(Ok(new ResponseDto { message = "", status ="Success", statusText = "Password and user details updated successfully" }));
                                    }
                                    else
                                    {
                                        return new JsonResult(BadRequest(new ResponseDto { statusText = "Failed to                  update user details", status="Error", message="" }));
                                    }
                                }
                                else
                                {
                                    return new JsonResult(BadRequest(new ResponseDto { statusText = "Failed to reset password", status = "Error", message = ""  }));
                                }
                            }
                            else
                            {
                            return new JsonResult(BadRequest(new ResponseDto{ statusText = "Old password is incorrect", status="Error", message ="" }));
                            }

                    }
                    else
                    {
                        throw new Exception("User not found");
                    }

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
                    var currentUser = await userManager.FindByEmailAsync(user.email);

                    if (currentUser != null)
                    {

                        var hashedAnswer = Helper.HashString(user.answer1);

                        var userQuestion = await _dcimDevContext.Questions
                           .FirstOrDefaultAsync(q => q.user_id == currentUser.Id
                                                     && q.question == user.question1
                                                     && q.answerHash.SequenceEqual(hashedAnswer)); 

                        if (userQuestion != null)
                        {

                                var token = await userManager.GeneratePasswordResetTokenAsync(currentUser);
                                var passwordResetResult = await userManager.ResetPasswordAsync(currentUser, token, user.password);

                                if (passwordResetResult.Succeeded)
                                {

                                    currentUser.DisplayName = user.displayName;
                                    currentUser.UserName = user.userName;
                                    currentUser.Email = user.email;

                                    var updateResult = await userManager.UpdateAsync(currentUser);

                                    if (updateResult.Succeeded)
                                    {
                                        return new JsonResult(Ok(new ResponseDto { message = "", status = "Success",             statusText = "Password and user details updated successfully" }));
                                     }
                                    else
                                    {
                                    return new JsonResult(BadRequest(new ResponseDto { statusText = "Failed to                      update user details", status = "Error", message = "" }));
                                    }
                                }
                                else
                                {
                                     return new JsonResult(BadRequest(new ResponseDto { statusText = "Failed to reset       password", status = "Error", message = "" }));
                                  }
                         }else
                            {
                            return new JsonResult(BadRequest(new ResponseDto { statusText = "Question or answer is wrong!", status = "Error", message = "" }));
                            }
                        }

                    else
                    {
                        throw new Exception("User not found");
                    }

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
                    var userExists = await userManager.FindByNameAsync(model.userName);
                    if (userExists != null)
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { status = "Error", message = "User already exists!" });

                    ApplicationUser user = new ApplicationUser()
                    {
                        Email = model.email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.userName
                    };
                    var result = await userManager.CreateAsync(user, model.password);
                    if (!result.Succeeded)
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { status = "Error", message = "User creation failed! Please check user details and try again." });

                    if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                    if (!await roleManager.RoleExistsAsync(UserRoles.User))
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                    if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        await userManager.AddToRoleAsync(user, UserRoles.Admin);
                    }

                    return Ok(new ResponseDto { status = "Success", message = "User created successfully!" });
                }
                catch (Exception ex) 
                {
                
                }

            }
        }
    }
}
