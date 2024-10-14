namespace DemoDotNetCoreApplication.Controllers
{
    using DemoDotNetCoreApplication.Dtos;
    using DemoDotNetCoreApplication.Modals;
    using DemoDotNetCoreApplication.Modals.JWTAuthentication.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

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

            public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
            {
                this.userManager = userManager;
                this.roleManager = roleManager;
                _configuration = configuration;
                this.signInManager = signInManager;
            }



            [HttpPost]
            [Route("login")]
            public async Task<IActionResult> Login([FromBody] LoginModalDto model)
            {
                var user = await userManager.FindByEmailAsync(model.UserName);

                var newname  = user.DisplayName;
                IList<string> gUserRoles;
                IList<Claim> gUserClaims;
                IList<Claim>? gRoleClaims = null;
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    
                    var userRoles = await userManager.GetRolesAsync(user);
                    gUserRoles = userRoles;
                    var userClaims = await userManager.GetClaimsAsync(user);
                    gUserClaims = userClaims;
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                       
                        var role = await roleManager.FindByNameAsync(userRole);
                       
                        var roleClaims = await roleManager.GetClaimsAsync(role);

                   
                        foreach (var roleClaim in roleClaims)
                        {
                            authClaims.Add(roleClaim);
                            gRoleClaims = roleClaims;
                        }
                    }

                    
                    foreach (var userClaim in userClaims)
                    {
                        authClaims.Add(userClaim);
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        expires: DateTime.Now.AddHours(3), // Token expiration time
                        claims: authClaims, // All claims (roles + custom claims + role claims)
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    UserContextDto userContextDto = new UserContextDto { User = user, Roles = gUserRoles, UserClaims = gUserClaims, RoleClaims = gRoleClaims };

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        userContext = userContextDto
                    });
                }
                return Unauthorized("Wrong email or passord");
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
                    var userExists = await userManager.FindByEmailAsync(model.email);

                    if (userExists != null)
                      return  new JsonResult(NotFound(new ResponseDto { status = "Error", statusText = "Duplicate email id, User already exists!", message = "" }));

                    ApplicationUser user = new ApplicationUser()
                    {
                        Email = model.email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.userName,
                        DisplayName = model.displayName
                    };

                var  result =  await userManager.CreateAsync(user, model.password);

                    if (!result.Succeeded)
                    {
                        return new JsonResult(BadRequest(result));
                    }

                }
                catch (Exception ex)
                {
                    return new JsonResult(StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { status = "Error", message = ex.Message }));
                }

                return Ok(new ResponseDto { status = "Success", message = "User created successfully!" });
            }

            [HttpPost]
            [Route("logout")]
            public async Task<IActionResult> Logout()
            {
                await signInManager.SignOutAsync(); 

                return Ok(new ResponseDto { status = "Success", message = "User logged out successfully!" });
            }

            [HttpPost]
            //[Authorize(Roles = "Admin")]
            //[Authorize(Policy ="ManageSystem")]
            [Route("register-admin")]
            public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModelDto model)
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
        }
    }
}
