using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using DemoDotNetCoreApplication.Modals.JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace DemoDotNetCoreApplication.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {

        private IServiceProvider _serviceProvider;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
      public  AdminController(IServiceProvider serviceProvider) {
            this._serviceProvider = serviceProvider;
            this._userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            this._roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }


        [HttpPost]
        [Route("add_role")]
        public async Task<IActionResult> AddRole([FromQuery] string roleName)
        {

                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                    return Ok("Role added to role successfully");

                }
                else
                {
                    return Conflict("Role already exist");
                }
            
        }


        [HttpPost]
        [Route("add_role_to_user")]
        public async Task<IActionResult> AddUserToRole([FromBody]  AssignRoleDto role)
        {
            var user = await _userManager.FindByIdAsync(role.userId);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.roleName);
                if (result.Succeeded)
                {
                    return Ok("User added to role successfully");
                }
            }
            return BadRequest("Failed to add user to role");
        }

        [HttpPost]
        [Route("add_claim_to_user")]
        public async Task<IActionResult> AddClaimToUser([FromBody] AssignClaimDto userClaim)
        {
            var user = await _userManager.FindByIdAsync(userClaim.userId);
            if (user != null)
            {
                var claim = new Claim(userClaim.claimType, userClaim.claimValue);
                var result = await _userManager.AddClaimAsync(user, claim);
                if (result.Succeeded)
                {
                    return Ok("Claim added to user");
                }
            }
            return BadRequest("Failed to add claim to user");
        }

        [HttpPost]
        [Route("add_claim_to_role")]
        public async Task<IActionResult> AddClaimToRole(string roleName,ClaimDto claimDto)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }

            Claim claim = new Claim(claimDto.ClaimType, claimDto.ClaimValue);
            var result = await _roleManager.AddClaimAsync(role, claim);

            if (result.Succeeded)
            {
                return Ok("Claim added to role");
            }

            return BadRequest("Could not add claim");
        }

        [HttpGet]
        [Route("getRoles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseDto { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var roles = await _userManager.Users.ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseDto { Status = "Error", Message = ex.Message });
            }
        }

    }


}
