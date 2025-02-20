﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Helpers;
using SMEConnect.Modals;

namespace SMEConnect.Controllers
{
    [Authorize(AuthenticationSchemes = "CustomJwt, AzureAD")]
    public class GroupRequestController : ControllerBase
    {
        private IGroupRequestProvider _groupProvider;
        private IAuthenticationProvider _authenticationProvider;

        public GroupRequestController(IGroupRequestProvider groupRequestProvider,IAuthenticationProvider authenticationProvider) 
        {
            _groupProvider = groupRequestProvider;  
            _authenticationProvider = authenticationProvider;
                
        }

        [HttpPost]
        [Route("add_group_request")]
        public async Task<IActionResult> AddGroup([FromBody] GroupRequest group)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                group.ModifiedBy = userContext.Email;

                var result = await _groupProvider.AddUserRequests(group);

                if (result.Status == Constants.ApiResponseType.Failure)
                {
                    return new JsonResult(NotFound(result));
                }
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }

        }

        [HttpPost]
        [Route("update_group_request")]
        public async Task<IActionResult> UpdateGroupRequest([FromBody] GroupRequest group)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                group.ModifiedBy = userContext.Email;

                var isAdminOrLead = await Helper.IsGroupLeadAsync(_authenticationProvider, userContext);
                if (isAdminOrLead)
                {
                    var result = await _groupProvider.UpdateUserRequests(group);

                    if (result.Status == Constants.ApiResponseType.Failure)
                    {
                        return new JsonResult(NotFound(result));
                    }
                    return new JsonResult(Ok(result));

                }
                else
                {
                    return new JsonResult(Unauthorized());
                }
                }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }

        }

        [HttpGet]
        [Route("get_group_requests")]
        public async Task<IActionResult> GetGroups([FromQuery] string userEmail)
        {
            try
            {
                var result = await this._groupProvider.GetUserRequests(userEmail);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }


        [HttpDelete]
        [Route("delete_group_requests")]
        public async Task<IActionResult> DeleteGroup(List<int> groupIds)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                var isAdminOrLead = await Helper.IsGroupLeadAsync(_authenticationProvider, userContext);
                if (isAdminOrLead)
                {
                    var result = await this._groupProvider.DelereGroupRequest(groupIds);
                    return new JsonResult(Ok(result));
                }
                else
                {
                    return new JsonResult(Unauthorized());
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }

    }
}
