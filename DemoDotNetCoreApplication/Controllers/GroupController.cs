using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Providers;
using DemoDotNetCoreApplication.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DemoDotNetCoreApplication.Modals;

namespace DemoDotNetCoreApplication.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {

        private UserGroupProvider _userGroupProvider;
        private IUserContext _userContext;

        public GroupController( UserGroupProvider userGroupProvider,IUserContext userContext)
        {
            this._userGroupProvider = userGroupProvider;
            _userContext = userContext;
        }


        [HttpPost]
        [Route("add_group")]
        public async Task<IActionResult> AddGroup([FromBody] UserGroup group)
        {

            try
            {
                group.ModifiedBy = _userContext.Email;
                var result = await this._userGroupProvider.AddGroup(group);
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
        [Route("delete_group")]
        public async Task<IActionResult> DeleteGroup([FromBody] List<UserGroup> groupIds)
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

    }
}