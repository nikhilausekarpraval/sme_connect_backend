using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Modals;
using static DemoDotNetCoreApplication.Constatns.Constants;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface IGroupProvider
    {
        public Task<string> AddGroup(UserGroup userGroup);


        public Task<ApiResponse<List<UserGroup>>> getGroups();


        public Task<ApiResponse<bool>> DeleteUserGroup(List<UserGroup> ids);

    }
}
