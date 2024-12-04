using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoDotNetCoreApplication.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class PracticeController : ControllerBase
    {

        private IAdminProvider _adminProvider;

        public PracticeController(IServiceProvider serviceProvider, IAdminProvider adminProvider)
        {
            this._adminProvider = adminProvider;
        }


        [HttpPost]
        [Route("add_practices")]
        public async Task<IActionResult> AddPractice([FromBody] RoleDto role)
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
        [Route("get_practices")]
        public async Task<IActionResult> GetPracticess()
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
        //[Route("delete_practices")]
        //public async Task<IActionResult> DeletePractices([FromBody] List<int> practicesIds)
        //{
        //    try
        //    {
        //        var result = await this._adminProvider.DeleteUser(practicesIds);
        //        return new JsonResult(Ok(result));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult(NotFound(ex));
        //    }
        //}
    }

}
