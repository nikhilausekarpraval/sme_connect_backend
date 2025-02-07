using SMEConnect.Modals;

namespace SMEConnect.Contracts
{
    public interface IDiscussionChatProvider
    {
        public Task<ApiResponse<List<DiscussionChat>>> getDiscussionChat(string groupId);

        public Task<ApiResponse<bool>> DeleteDiscussionChat(List<int> discussionChat);

        public Task<ApiResponse<bool>> CreateDiscussionChat(DiscussionChat DiscussionChat);

        public Task<ApiResponse<bool>> UpdateDiscussionChat(DiscussionChat discussionChat);
    }
}
