﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Contracts;
using SMEConnect.Dtos;
using static SMEConnect.Constatns.Constants;


namespace SMEConnect.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {

        private IAdminProvider _adminProvider;

        public AdminController(IServiceProvider serviceProvider, IAdminProvider adminProvider)
        {
            this._adminProvider = adminProvider;
        }


        [HttpPost]
        [Route("add_role")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto role)
        {

            try
            {
                var result = await this._adminProvider.AddRole(role);
                return result == AccessConfigurationErrorMessage.RoleExist ? new JsonResult(NotFound(result)) : new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }

        }


        [HttpPost]
        [Route("add_role_to_user")]
        public async Task<IActionResult> AddRoleToUser([FromBody] AssignRoleDto addRoleToUser)
        {
            try
            {
                var result = await this._adminProvider.AddRoleToUser(addRoleToUser);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpPost]
        [Route("add_claim_to_user")]
        public async Task<IActionResult> AddClaimToUser([FromBody] AssignClaimDto userClaim)
        {
            try
            {
                var result = await this._adminProvider.AddClaimToUser(userClaim);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpGet]
        [Route("get_role_with_claims")]
        public async Task<IActionResult> GetRolesWithclaims()
        {
            try
            {
                var result = await this._adminProvider.GetRolesWithClaims();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }



        [HttpGet]
        [Route("get_roles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var result = await this._adminProvider.GetRoles();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }


        [HttpDelete]
        [Route("delete_role")]
        public async Task<IActionResult> DeleteRole([FromBody] List<string> roles)
        {
            try
            {
                var result = await this._adminProvider.DeleteRoles(roles);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }


        [HttpGet]
        [Route("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await this._adminProvider.GetUsers();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpDelete]
        [Route("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] List<string> applicationUsers)
        {
            try
            {
                var result = await this._adminProvider.DeleteUser(applicationUsers);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

    }

}
