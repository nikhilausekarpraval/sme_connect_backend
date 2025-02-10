﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Modals;

namespace SMEConnect.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class GroupUsersController : ControllerBase
    {

        private IGroupUserProvider _userGroupUsersProvider;

        public GroupUsersController(IGroupUserProvider userGroupUsersProvider)
        {
            this._userGroupUsersProvider = userGroupUsersProvider;
        }

        //update here access
        [HttpPost]
        [Route("add_group_user")]
        public async Task<IActionResult> AddGroupUsers(GroupUser group)
        {

            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                group.ModifiedBy = userContext.Email;
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

        [HttpGet]
        [Route("get_group_all_users")]
        public async Task<IActionResult> GetGroupAllUsers([FromQuery] string group)
        {
            try
            {
                var result = await this._userGroupUsersProvider.getGroupAllUsers(group);
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
        public async Task<IActionResult> getUserGroups([FromQuery] string practice)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                var result = await this._userGroupUsersProvider.GetUserGroups(userContext.Email,practice);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("update_group_user")]
        public async Task<IActionResult> UpdateGroupUsers(GroupUser group)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                group.ModifiedBy = userContext.Email;
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