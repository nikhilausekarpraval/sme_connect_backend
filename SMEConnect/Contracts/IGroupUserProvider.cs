using SMEConnect.Dtos;
using SMEConnect.Modals;

namespace SMEConnect.Contracts
{
    public interface IGroupUserProvider
    {
        public Task<ApiResponse<bool>> AddGroupUser(GroupUser userGroupUser);

        public Task<ApiResponse<List<GroupUser>>> getGroupUsers();

        public Task<ApiResponse<bool>> DeleteGroupUser(List<int> ids);

        public Task<ApiResponse<bool>> UpdateGroupUser(GroupUser group);

        public Task<ApiResponse<List<GroupUser>>> GetUserGroups(string practice);

        public Task<ApiResponse<List<GroupUserDto>>> getGroupAllUsers(string group);
    }
}
