using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace DemoDotNetCoreApplication.Controllers
{
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [Route("employee")]
    public class AdminController : Controller
    {

        private IServiceProvider _serviceProvider;
        private UserManager<IdentityUser> _userManager;
        AdminController(IServiceProvider serviceProvider) {
            this._serviceProvider = serviceProvider;
            this._userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        }

        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return Ok("User added to role successfully");
                }
            }
            return BadRequest("Failed to add user to role");
        }

        public async Task<IActionResult> AddClaimToUser(string userId, string claimType, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var claim = new Claim(claimType, claimValue);
                var result = await _userManager.AddClaimAsync(user, claim);
                if (result.Succeeded)
                {
                    return Ok("Claim added to user");
                }
            }
            return BadRequest("Failed to add claim to user");
        }

    }


}
