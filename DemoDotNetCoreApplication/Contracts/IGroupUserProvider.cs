using DemoDotNetCoreApplication.Modals;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface IGroupUserProvider
    {
        public Task<ApiResponse<bool>> AddGroupUser(GroupUser userGroupUser);

        public Task<ApiResponse<List<GroupUser>>> getGroupUsers();

        public Task<ApiResponse<bool>> DeleteGroupUser(List<int> ids);

        public Task<ApiResponse<bool>> UpdateGroupUser(GroupUser group);
    }
}
