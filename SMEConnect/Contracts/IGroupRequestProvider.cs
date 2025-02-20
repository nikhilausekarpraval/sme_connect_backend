using SMEConnect.Data;
using SMEConnect.Modals;

namespace SMEConnect.Contracts
{
    public interface IGroupRequestProvider
    {
        public Task<ApiResponse<List<GroupRequest>>> GetUserRequests(string userEmail);

        public Task<ApiResponse<bool>> UpdateUserRequests(GroupRequest groupRequest);

        public Task<ApiResponse<bool>> DelereGroupRequest(List<int> groupRequestIds);

        public Task<ApiResponse<bool>> AddUserRequests(GroupRequest groupRequest);
    }
}
