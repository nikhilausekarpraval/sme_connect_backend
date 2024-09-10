using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface ITaskItemProvider
    {
        public  Task<ApiResponse<List<TaskItem>>> getTaskItems();

        public  Task<ApiResponse<TaskItem>> GetTaskItem(int id);

        public  Task<ApiResponse<bool>> DeleteTaskItem(int id);

        public  Task<ApiResponse<bool>> CreateTaskItem(TaskItem employee);

        public  Task<ApiResponse<bool>> UpdateTaskItem(TaskItem employee);

    }
}
