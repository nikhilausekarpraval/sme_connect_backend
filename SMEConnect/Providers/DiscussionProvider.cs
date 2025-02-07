using Microsoft.EntityFrameworkCore;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Modals;

namespace SMEConnect.Providers
{

    public class DiscussionProvider : IDiscussionProvider
    {
        private readonly DcimDevContext _context;
        private readonly ILogger _logger;

        public DiscussionProvider(DcimDevContext context, ILogger<DiscussionProvider> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<List<Discussion>>> getDiscussions(string groupId)
        {
            try
            {
                var discussion = await _context.Discussions.Where(d => d.GroupName == groupId).ToListAsync();
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Success, discussion, "");
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }


        public async Task<ApiResponse<bool>> DeleteDiscussion(string discussionsIds)
        {
            try
            {
                var discussionsToRemove = _context.Discussions.Where(discussion => discussion.Name == discussionsIds).ToList();

                if (discussionsToRemove.Any())
                {
                    _context.Discussions.RemoveRange(discussionsToRemove);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                else
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "No matching discussion found.");
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }


        public async Task<ApiResponse<List<Discussion>>> getRecentDiscussions(string discussion)
        {
            try
            {
                var discussions = await _context.Discussions.Where(d => d.GroupName == discussion).ToListAsync();
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Success, discussions, "");
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }

        public async Task<ApiResponse<List<Discussion>>> GetSimilerDiscussionFromGroup(string discussion)
        {
            try
            {
                var discussions = await _context.Discussions.Where(d => d.GroupName == discussion).ToListAsync();
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Success, discussions, "");
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateDiscussion(Discussion discussion)
        {
            try
            {
                bool exists = await _context.Discussions.AnyAsync(p => p.Name == discussion.Name);
                if (exists)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "A discussion with the same name already exists.");
                }

                await _context.Discussions.AddAsync(discussion);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateDiscussion(Discussion discussion)
        {
            try
            {
                var existingdiscussion = await _context.Discussions.FindAsync(discussion.Name);
                if (existingdiscussion == null)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "Selected discussion not found.");
                }

                existingdiscussion.Name = discussion.Name;
                existingdiscussion.Description = discussion.Description;

                _context.Discussions.Update(existingdiscussion);
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
