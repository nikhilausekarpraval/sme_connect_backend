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
    public class PracticeController : ControllerBase
    {

        private IPracticeProvider _practiceProvider;
        private IUserContext _userContext;

        public PracticeController(IPracticeProvider practiceProvider,IUserContext userContext)
        {
            this._practiceProvider = practiceProvider;
            this._userContext = userContext;
        }


        [HttpPost]
        [Route("add_practice")]
        public async Task<IActionResult> AddPractice(Practice practice)
        {

            try
            {
                practice.ModifiedBy = _userContext.Email;
                var result = await this._practiceProvider.CreatePractice(practice);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }

        }
        [HttpGet]
        [Route("get_practices")]
        public async Task<IActionResult> GetPractices()
        {
            try
            {
                var result = await this._practiceProvider.getPractices();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }


        [HttpDelete]
        [Route("delete_practices")]
        public async Task<IActionResult> DeletePractices( List<int> practicesIds)
        {
            try
            {
                var result = await this._practiceProvider.DeletePractice(practicesIds);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }
    }

}
