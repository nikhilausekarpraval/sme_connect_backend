﻿using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace DemoDotNetCoreApplication.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {

        private IServiceProvider _serviceProvider;
        private UserManager<ApplicationUser> _userManager;
      public  AdminController(IServiceProvider serviceProvider) {
            this._serviceProvider = serviceProvider;
            this._userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        }

        [HttpPost]
        [Route("add_role")]
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
        [Route("add_claim")]
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

    }


}
