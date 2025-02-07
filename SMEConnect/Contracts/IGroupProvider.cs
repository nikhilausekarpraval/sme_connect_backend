using SMEConnect.Data;
using SMEConnect.Modals;
using static SMEConnect.Constatns.Constants;

namespace SMEConnect.Contracts
{
    public interface IGroupProvider
    {
        public Task<ApiResponse<bool>> AddGroup(UserGroup userGroup);


        public Task<ApiResponse<List<UserGroup>>> getGroups();

        public Task<ApiResponse<List<UserGroup>>> getUserPracticeGroups(string practice="");

        public Task<ApiResponse<bool>> DeleteUserGroup(List<int> ids);

        public  Task<ApiResponse<bool>> UpdateGroup(UserGroup group);

    }
}
