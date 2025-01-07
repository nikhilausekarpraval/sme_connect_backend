using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Providers;
using DemoDotNetCoreApplication.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DemoDotNetCoreApplication.Modals;
using DemoDotNetCoreApplication.Constatns;

namespace DemoDotNetCoreApplication.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {

        private IGroupProvider _userGroupProvider;
        private IUserContext _userContext;

        public GroupController(IGroupProvider userGroupProvider,IUserContext userContext)
        {
            this._userGroupProvider = userGroupProvider;
            _userContext = userContext;
        }


        [HttpPost]
        [Route("add_group")]
        public async Task<IActionResult> AddGroup( UserGroup group)
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


        [HttpDelete]
        [Route("delete_groups")]
        public async Task<IActionResult> DeleteGroup( List<int> groupIds)
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