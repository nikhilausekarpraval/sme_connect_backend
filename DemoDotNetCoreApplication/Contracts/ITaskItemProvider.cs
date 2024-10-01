using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface ITaskItemProvider
    {
        public  Task<ApiResponse<List<Modals.Task>>> getTaskItems();

        public  Task<ApiResponse<Modals.Task>> GetTaskItem(int id);

        public  Task<ApiResponse<bool>> DeleteTaskItem(int id);

        public  Task<ApiResponse<bool>> CreateTaskItem(Modals.Task employee);

        public  Task<ApiResponse<bool>> UpdateTaskItem(Modals.Task employee);

    }
}
