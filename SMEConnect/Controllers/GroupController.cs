using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Modals;

namespace SMEConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {

        private IGroupProvider _userGroupProvider;
        private IUserContext _userContext;

        public GroupController(IGroupProvider userGroupProvider, IUserContext userContext)
        {
            this._userGroupProvider = userGroupProvider;
            _userContext = userContext;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("add_group")]
        public async Task<IActionResult> AddGroup(UserGroup group)
        {

            try
            {
                group.ModifiedBy = _userContext.Email;
                var result = await this._userGroupProvider.AddGroup(group);
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
        [Route("get_groups")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetGroups()
        {
            try
            {
                var result = await this._userGroupProvider.getGroups();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpGet]
        [Route("get_user_practice_groups")]
        public async Task<IActionResult> GetUserPracticeGroups([FromQuery] string practice)
        {
            try
            {
                var roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                var userRole = roleClaim?.Value;

                var result = await this._userGroupProvider.getUserPracticeGroups(practice);

                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }



        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("delete_groups")]
        public async Task<IActionResult> DeleteGroup(List<int> groupIds)
        {
            try
            {
                var result = await this._userGroupProvider.DeleteUserGroup(groupIds);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpPost]
        [Route("update_group")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateGroup(UserGroup group)
        {
            try
            {
                group.ModifiedBy = _userContext.Email;
                var result = await this._userGroupProvider.UpdateGroup(group);
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