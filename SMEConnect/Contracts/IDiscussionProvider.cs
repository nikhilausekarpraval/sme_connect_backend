using DemoDotNetCoreApplication.Modals;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface IDiscussionProvider
    {
        public Task<ApiResponse<List<Discussion>>> getDiscussions (string groupId);

        public Task<ApiResponse<bool>> DeleteDiscussion(string discussions);

        public Task<ApiResponse<bool>> CreateDiscussion(Discussion Discussion);

        public Task<ApiResponse<bool>> UpdateDiscussion(Discussion discussion);

    }
}
