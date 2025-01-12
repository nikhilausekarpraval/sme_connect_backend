using DemoDotNetCoreApplication.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DemoDotNetCoreApplication.Modals;
using DemoDotNetCoreApplication.Constatns;

namespace DemoDotNetCoreApplication.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class GroupUsersController : ControllerBase
    {

        private IGroupUserProvider _userGroupUsersProvider;
        private IUserContext _userContext;

        public GroupUsersController(IGroupUserProvider userGroupUsersProvider, IUserContext userContext)
        {
            this._userGroupUsersProvider = userGroupUsersProvider;
            _userContext = userContext;
        }


        [HttpPost]
        [Route("add_group_user")]
        public async Task<IActionResult> AddGroupUsers(GroupUser group)
        {

            try
            {
                group.ModifiedBy = _userContext.Email;
                var result = await this._userGroupUsersProvider.AddGroupUser(group);
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
        [Route("get_group_users")]
        public async Task<IActionResult> GetGroupUsers()
        {
            try
            {
                var result = await this._userGroupUsersProvider.getGroupUsers();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }


        [HttpDelete]
        [Route("delete_group_users")]
        public async Task<IActionResult> DeleteGroupUsers(List<int> groupIds)
        {
            try
            {
                var result = await this._userGroupUsersProvider.DeleteGroupUser(groupIds);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpGet]
        [Route("get_user_groups")]
        public async Task<IActionResult> getUserGroups()
        {
            try
            {
                var result = await this._userGroupUsersProvider.getUserGroups();
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpPost]
        [Route("update_group_user")]
        public async Task<IActionResult> UpdateGroupUsers(GroupUser group)
        {
            try
            {
                group.ModifiedBy = _userContext.Email;
                var result = await this._userGroupUsersProvider.UpdateGroupUser(group);
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