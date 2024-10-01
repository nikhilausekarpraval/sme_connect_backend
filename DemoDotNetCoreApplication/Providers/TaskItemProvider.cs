using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Formats.Asn1;

namespace DemoDotNetCoreApplication.Providers
{
    public class TaskItemProvider : ITaskItemProvider
    {
        private readonly DbContextProvider _context;

        public TaskItemProvider(DbContextProvider context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<ApiResponse<List<TaskItem>>> getTaskItems()
        {
            try
            {
                var taskItems = await _context.Tasks.ToListAsync(); 
                return new ApiResponse<List<TaskItem>>(Constants.ApiResponseType.Success, taskItems);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TaskItem>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }

        public async Task<ApiResponse<TaskItem>> GetTaskItem(int id)
        {
            try
            {
                var taskItem = await _context.Tasks.Include(e => e.employee).Where(e => e.employee.id == e.employeeId).FirstAsync();
                if (taskItem != null)
                {
                    return new ApiResponse<TaskItem>(Constants.ApiResponseType.Success, taskItem);
                }
                return new ApiResponse<TaskItem>(Constants.ApiResponseType.Success, null, "TaskItem not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<TaskItem>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteTaskItem(int id)
        {
            try
            {
                var taskItem = await _context.Tasks.FindAsync(id);
                if (taskItem != null)
                {
                     _context.Tasks.Remove(taskItem);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, false, "TaskItem not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateTaskItem(TaskItem taskItem)
        {
            try
            {
               await _context.Tasks.AddAsync(taskItem);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateTaskItem(TaskItem taskItem)
        {
            try
            {
                var existingTaskItem = await _context.Tasks.FindAsync(taskItem.id);
                if (existingTaskItem != null)
                {
                    _context.Entry(existingTaskItem).CurrentValues.SetValues(taskItem);
                    _context.SaveChanges();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "TaskItem not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }
    }

}
