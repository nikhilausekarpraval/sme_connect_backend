
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMEConnectSignalRServer.Dtos;
using SMEConnectSignalRServer.Modals;
using SMEConnectSignalRServer.Services;

namespace SMEConnectSignalRServer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private MessagesService _messageService;

        MessageController(MessagesService messageService) {
            _messageService = messageService;
        }

        [HttpGet]
        [Route("get-discussion-chat")]
        [Authorize]
        public async Task<IActionResult> GetDiscussionMessages([FromBody] UserDto userDto)
        {
            try
            {
                var result = await this._messageService.GetDiscussionChat(userDto);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpPost]
        [Route("add-message")]
        [Authorize]
        public async Task<IActionResult> AddMessage([FromForm] MessageDto messageDto, [FromForm] List<IFormFile> attachments)
        {
            try
            {
                var message = new Message
                {
                    Text = messageDto.Text,
                    UserName = messageDto.UserName,
                    CreatedDate = DateTime.Now,
                    Discussion = messageDto.Discussion,
                    Group = messageDto.Group,
                    Practice = messageDto.Practice,
                    Attachments = new List<FileAttachment>()
                };

                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);
                        message.Attachments.Add(new FileAttachment
                        {
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            Content = memoryStream.ToArray()
                        });
                    }
                }

                var result = await _messageService.AddMessage(message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("delete-message")]
        [Authorize]
        public async Task<IActionResult> DeleteMessage([FromBody] List<int> messageIds)
        {
            try
            {
                var result = await this._messageService.DeleteMessagesByFilter(messageIds);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }

        [HttpGet]
        [Route("update-message")]
        [Authorize]
        public async Task<IActionResult> UpdateMessage([FromBody] Message message)
        {
            try
            {
                var result = await this._messageService.UpdateMessage(message);
                return new JsonResult(Ok(result));
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex));
            }
        }
    }
}
