using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DemoDotNetCoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")] 
    public class UseAuthorizationController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UseAuthorizationController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new ApplicationRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return Ok($"Role {roleName} created");
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Role already exists");
        }

        [HttpGet("all")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles;
            return Ok(roles);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(string userId, string role, [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                {
                    return Ok($"Role {role} assigned to user {user.UserName}");
                }
                return BadRequest(result.Errors);
            }
            return NotFound("User not found");
        }

        [HttpGet("is-in-role")]
        public async Task<IActionResult> IsUserInRole(string userId, string role, [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var isInRole = await userManager.IsInRoleAsync(user, role);
                return Ok(isInRole);
            }
            return NotFound("User not found");
        }

        [HttpPost("add-claim")]
        public async Task<IActionResult> AddClaim(string userId, [FromServices] UserManager<IdentityUser> userManager)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var claim = new Claim("Department", "HR");
                var result = await userManager.AddClaimAsync(user, claim);

                if (result.Succeeded)
                {
                    return Ok("Claim added successfully");
                }
                return BadRequest(result.Errors);
            }
            return NotFound("User not found");
        }

    }
}
