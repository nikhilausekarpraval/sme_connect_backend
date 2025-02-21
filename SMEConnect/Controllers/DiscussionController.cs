using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Dtos;
using SMEConnect.Helpers;
using SMEConnect.Modals;
using SMEConnectSignalRServer.Dtos;

namespace SMEConnect.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = "CustomJwt, AzureAD")]
    [Route("api/[controller]")]
    public class DiscussionController : ControllerBase
    {

        private IDiscussionProvider _discussionProvider;
        private IAuthenticationProvider _authenticationProvider;

        public DiscussionController(IDiscussionProvider discussionProvider,IAuthenticationProvider authenticationProvider)
        {
            this._discussionProvider = discussionProvider;
            _authenticationProvider = authenticationProvider;
        }

        [HttpPost]
        [Route("add_discussion")]
        public async Task<IActionResult> AddDiscussion(Discussion discussion)
        {

            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                var groupRole = await _authenticationProvider.GetUserGroupRole(userContext.Email,discussion.GroupName);
                if (groupRole == "Lead"! || !userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                discussion.ModifiedBy = userContext.Email;
                var result = await this._discussionProvider.CreateDiscussion(discussion);
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

        [HttpGet]
        [Route("get_discussions")]
        public async Task<IActionResult> GetDiscussions([FromQuery] string groupName)
        {
            try
            {
                var result = await this._discussionProvider.getDiscussions(groupName);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }

        [HttpDelete]
        [Route("delete_discussion")]
        public async Task<IActionResult> DeleteDiscussion([FromBody] Discussion discussion)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                var groupRole = await _authenticationProvider.GetUserGroupRole(userContext.Email,discussion.GroupName);
                if (groupRole == "Lead"! || !userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                var result = await this._discussionProvider.DeleteDiscussion(discussion.Name);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }


        [HttpPost]
        [Route("get_recent_discussions")]
        public async Task<IActionResult> GetRecentDiscussionFromGroup([FromBody] DiscussionsDTO discussionsDTO)
        {
            try
            {
                var result = await this._discussionProvider.GetRecentDiscussions(discussionsDTO, Helper.GetAccessToken(this.HttpContext));
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }

        [HttpPost]
        [Route("get_discussion_users")]
        public async Task<IActionResult> GetDiscussionUsers([FromBody] DiscussionsDTO discussionsDTO)
        {
            try
            {
                var result = await this._discussionProvider.GetDiscussionUsers(discussionsDTO,Helper.GetAccessToken(this.HttpContext));
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }

        [HttpPost]
        [Route("get_similer_discussions")]
        public async Task<IActionResult> GetSimilerDiscussionFromGroup([FromBody] DiscussionsDTO discussionsDTO)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                
                var result = await this._discussionProvider.GetSimilarDiscussionsFromGroup(discussionsDTO,userContext.Email, Helper.GetAccessToken(this.HttpContext));
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }
        }

        [Authorize(AuthenticationSchemes = "CustomJwt, AzureAD")]
        [HttpPost]
        [Route("update_discussion")]
        public async Task<IActionResult> UpdateDiscussion(Discussion discussion)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                var groupRole = await _authenticationProvider.GetUserGroupRole(userContext.Email,discussion.GroupName);
                if (groupRole == "Lead"! || !userContext.Roles.Contains("Admin"))
                {
                    return Unauthorized();
                }
                discussion.ModifiedBy = userContext.Email;
                var result = await this._discussionProvider.UpdateDiscussion(discussion);
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
    }
}
