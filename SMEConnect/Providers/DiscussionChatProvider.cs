using Microsoft.EntityFrameworkCore;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Modals;

namespace SMEConnect.Providers
{

    public class DiscussionChatProvider : IDiscussionChatProvider
    {
        private readonly DcimDevContext _context;
        private readonly ILogger _logger;

        public DiscussionChatProvider(DcimDevContext context, ILogger<DiscussionChatProvider> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<List<DiscussionChat>>> getDiscussionChat(string chatId)
        {
            try
            {
                var chat = await _context.DiscussionChat.Where(d => d.DiscussionName == chatId).ToListAsync();

                return new ApiResponse<List<DiscussionChat>>(Constants.ApiResponseType.Success, chat, "");
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<List<DiscussionChat>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }


        public async Task<ApiResponse<bool>> DeleteDiscussionChat(List<int> chatId)
        {
            try
            {
                var chatsToRemove = _context.DiscussionChat.Where(chat => chatId.Contains(chat.Id)).ToList();

                if (chatsToRemove.Any())
                {
                    _context.DiscussionChat.RemoveRange(chatsToRemove);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                else
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "No matching chat found.");
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateDiscussionChat(DiscussionChat chat)
        {
            try
            {
                await _context.DiscussionChat.AddAsync(chat);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateDiscussionChat(DiscussionChat chat)
        {
            try
            {
                var existingchat = await _context.DiscussionChat.FindAsync(chat.Id);
                if (existingchat == null)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "Selected chat not found.");
                }

                existingchat.Message = chat.Message;

                _context.DiscussionChat.Update(existingchat);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }
    }
}
