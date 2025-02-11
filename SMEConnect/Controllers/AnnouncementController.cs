using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Dtos;
using SMEConnect.Modals;

namespace SMEConnect.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private IAnnouncementProvider _announcementProvider;

        public AnnouncementController(IAnnouncementProvider announcementProvider)
        {
            this._announcementProvider = announcementProvider;
        }


        [HttpPost]
        [Route("add_announcement")]
        public async Task<IActionResult> AddAnnouncement(Announcement announcement)
        {
            try
            {
                var userContext = HttpContext.Items["UserContext"] as UserContext;
                announcement.CreatedBy = userContext.Email;
                var result = await this._announcementProvider.AddAnnouncement(announcement);
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

        [HttpPost]
        [Route("get_announcements")]
        public async Task<IActionResult> GetAnnouncements([FromBody] UserDetailsDto userDetailsDto)
        {
            try
            {
                var result = await this._announcementProvider.getUsersLast5DaysAnnouncements(userDetailsDto);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }
    }
}
