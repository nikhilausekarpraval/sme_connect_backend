using Microsoft.AspNetCore.Mvc;
using SMEConnectSignalRServer.Dtos;
using SMEConnectSignalRServer.Interfaces;
using SMEConnectSignalRServer.Modals;

namespace SMEConnectSignalRServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            this._messageService = messageService;
        }

        [HttpPost]
        [Route("get-discussion-chat")]
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
        public async Task<IActionResult> AddMessage([FromForm] MessageDto messageDto)
        {
            try
            {
                var message = new Message
                {
                    Text = messageDto.Text,
                    UserName = messageDto.UserName,
                    CreatedDate = DateTime.Now,
                    ReplyedTo = messageDto.ReplyedTo,
                    Discussion = messageDto.Discussion,
                    Group = messageDto.Group,
                    Practice = messageDto.Practice,
                    Attachments = new List<FileAttachment>(),
                    DisplayName = messageDto.DisplayName
                };

                if (messageDto.Attachments != null)
                {
                    foreach (var file in messageDto.Attachments)
                    {
                        if (file?.Length > 0)
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
                }

                var result = await _messageService.AddMessage(message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete-message")]
        public async Task<IActionResult> DeleteMessage([FromBody] List<dynamic> messageIds)
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

        [HttpPut]
        [Route("update-message")]
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
