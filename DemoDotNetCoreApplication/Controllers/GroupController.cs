using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoDotNetCoreApplication.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {

        private IAdminProvider _adminProvider;

        public GroupController(IServiceProvider serviceProvider, IAdminProvider adminProvider)
        {
            this._adminProvider = adminProvider;
        }


        [HttpPost]
        [Route("add_roup")]
        public async Task<IActionResult> AddGroup([FromBody] RoleDto role)
        {

            try
            {
                var result = await this._adminProvider.AddRole(role);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }

        }
        [HttpGet]
        [Route("get_groups")]
        public async Task<IActionResult> GetGroups()
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


        //[HttpDelete]
        //[Route("delete_group")]
        //public async Task<IActionResult> DeleteGroup([FromBody] List<int> groupIds)
        //{
        //    try
        //    {
        //        var result = await this._adminProvider.DeleteUser(groupIds);
        //        return new JsonResult(Ok(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult(NotFound(ex));
        //    }
        //}

    }
}