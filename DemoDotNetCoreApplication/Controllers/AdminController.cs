using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DemoDotNetCoreApplication.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {

      private IAdminProvider _adminProvider;

      public  AdminController(IServiceProvider serviceProvider,IAdminProvider adminProvider) {
            this._adminProvider = adminProvider;
        }


        [HttpPost]
        [Route("add_role")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto role)
        {

            try
            {
              var result =  await this._adminProvider.AddRole(role);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
            
        }


        [HttpPost]
        [Route("add_role_to_user")]
        public async Task<IActionResult> AddRoleToUser([FromBody]  AssignRoleDto addRoleToUser)
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

        [HttpPost]
        [Route("add_claim_to_role")]
        public async Task<IActionResult> AddClaimToRole([FromBody] AddClaimToRoleDto roleClaim )
        {
            try
            {
                var result = await this._adminProvider.AddClaimToRole(roleClaim);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpGet]
        [Route("getRoles")]
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
