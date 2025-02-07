
using SMEConnect.Modals;
using SMEConnectSignalRServer.Dtos;

namespace SMEConnect.Contracts
{
    public interface IDiscussionProvider
    {
        public Task<ApiResponse<List<Discussion>>> getDiscussions(string groupId);

        public Task<ApiResponse<bool>> DeleteDiscussion(string discussions);

        public Task<ApiResponse<List<Discussion>>> GetSimilarDiscussionsFromGroup(DiscussionsDTO discussion);

        public  Task<ApiResponse<List<Discussion>>> GetRecentDiscussions(DiscussionsDTO discussion);

        public Task<ApiResponse<bool>> CreateDiscussion(Discussion Discussion);

        public Task<ApiResponse<bool>> UpdateDiscussion(Discussion discussion);

    }
}
