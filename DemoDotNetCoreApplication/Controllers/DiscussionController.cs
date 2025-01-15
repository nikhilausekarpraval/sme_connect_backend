using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoDotNetCoreApplication.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class DiscussionController : ControllerBase
    {

        private IDiscussionProvider _discussionProvider;
        private IUserContext _userContext;

        public DiscussionController(IDiscussionProvider discussionProvider, IUserContext userContext)
        {
            this._discussionProvider = discussionProvider;
            this._userContext = userContext;
        }


        [HttpPost]
        [Route("add_discussion")]
        public async Task<IActionResult> AddDiscussion(Discussion discussion)
        {

            try
            {
                discussion.ModifiedBy = _userContext.Email;
                var result = await this._discussionProvider.CreateDiscussion(discussion);
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
                return new JsonResult(NotFound(ex));
            }
        }


        [HttpDelete]
        [Route("delete_discussion")]
        public async Task<IActionResult> DeleteDiscussion([FromBody] string discussion)
        {
            try
            {
                var result = await this._discussionProvider.DeleteDiscussion(discussion);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpPost]
        [Route("update_discussion")]
        public async Task<IActionResult> UpdateDiscussion(Discussion discussion)
        {
            try
            {
                discussion.ModifiedBy = _userContext.Email;
                var result = await this._discussionProvider.UpdateDiscussion(discussion);
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
