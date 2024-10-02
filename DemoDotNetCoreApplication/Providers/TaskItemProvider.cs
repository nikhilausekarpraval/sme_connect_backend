using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Formats.Asn1;

namespace DemoDotNetCoreApplication.Providers
{
    public class TaskItemProvider : ITaskItemProvider
    {
        private readonly DcimDevContext _context;

        public TaskItemProvider(DcimDevContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<ApiResponse<List<DemoDotNetCoreApplication.Modals.Task>>> getTaskItems()
        {
            try
            {
                var taskItems = await _context.Tasks.ToListAsync(); 
                return new ApiResponse<List<DemoDotNetCoreApplication.Modals.Task>>(Constants.ApiResponseType.Success, taskItems);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<DemoDotNetCoreApplication.Modals.Task>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }

        public async Task<ApiResponse<DemoDotNetCoreApplication.Modals.Task>> GetTaskItem(int id)
        {
            try
            {
                var taskItem = await _context.Tasks.Include(e => e.Employee).Where(e => e.Employee.Id == e.EmployeeId).FirstAsync();
                if (taskItem != null)
                {
                    return new ApiResponse<DemoDotNetCoreApplication.Modals.Task>(Constants.ApiResponseType.Success, taskItem);
                }
                return new ApiResponse<DemoDotNetCoreApplication.Modals.Task>(Constants.ApiResponseType.Success, null, "DemoDotNetCoreApplication.Modals.Task not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<DemoDotNetCoreApplication.Modals.Task>(Constants.ApiResponseType.Failure, null, ex.Message);
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
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, false, "DemoDotNetCoreApplication.Modals.Task not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateTaskItem(DemoDotNetCoreApplication.Modals.Task taskItem)
        {
            try
            {
                var task = taskItem;
                task.Id = 0;
                var emp = await _context.Employees.FindAsync(taskItem.EmployeeId);
                task.Employee = emp;
                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateTaskItem(DemoDotNetCoreApplication.Modals.Task taskItem)
        {
            try
            {
                var existingTaskItem = await _context.Tasks.FindAsync(taskItem.Id);
                if (existingTaskItem != null)
                {
                    _context.Entry(existingTaskItem).CurrentValues.SetValues(taskItem);
                    _context.SaveChanges();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "DemoDotNetCoreApplication.Modals.Task not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }
    }

}
