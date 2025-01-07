using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Modals;
using static DemoDotNetCoreApplication.Constatns.Constants;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface IGroupProvider
    {
        public Task<ApiResponse<bool>> AddGroup(UserGroup userGroup);


        public Task<ApiResponse<List<UserGroup>>> getGroups();


        public Task<ApiResponse<bool>> DeleteUserGroup(List<int> ids);

        public  Task<ApiResponse<bool>> UpdateGroup(UserGroup group);

    }
}
