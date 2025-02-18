using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Modals;

namespace SMEConnect.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = "CustomJwt, AzureAD")]
    [Route("api/[controller]")]
    public class PracticeController : ControllerBase
    {

        private IPracticeProvider _practiceProvider;

        public PracticeController(IPracticeProvider practiceProvider)
        {
            this._practiceProvider = practiceProvider;
    
        }


        [HttpPost]
        [Route("add_practice")]
        public async Task<IActionResult> AddPractice(Practice practice)
        {

            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                if (!userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                practice.ModifiedBy = userContext.Email;
                var result = await this._practiceProvider.CreatePractice(practice);
                if (result.Status == Constants.ApiResponseType.Failure)
                {
                    return new JsonResult(NotFound(result));
                }
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
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                if (!userContext.Roles.Contains("Admin"))
                {
                    var role = userContext.Roles[3];
                    return Unauthorized();
                }
                var result = await this._practiceProvider.getPractices();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpGet]
        [Route("get_practice")]
        public async Task<IActionResult> GetPractice([FromQuery] string id)
        {
            try
            {
                var result = await this._practiceProvider.getPractice(id);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }


        [HttpDelete]
        [Route("delete_practices")]
        public async Task<IActionResult> DeletePractices(List<int> practicesIds)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                if (!userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                var result = await this._practiceProvider.DeletePractice(practicesIds);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpPost]
        [Route("update_practice")]
        public async Task<IActionResult> UpdatePractice(Practice practice)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                if (!userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                practice.ModifiedBy = userContext.Email;
                var result = await this._practiceProvider.UpdatePractice(practice);
                if (result.Status == Constants.ApiResponseType.Failure)
                {
                    return new JsonResult(NotFound(result));
                }
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }
    }

}
