using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Modals;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Formats.Asn1;

namespace SMEConnect.Providers
{
    public class TaskItemProvider : ITaskItemProvider
    {
        private readonly DcimDevContext _context;

        public TaskItemProvider(DcimDevContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<ApiResponse<List<SMEConnect.Modals.Task>>> getTaskItems()
        {
            try
            {
                var taskItems = await _context.Tasks.ToListAsync(); 
                return new ApiResponse<List<SMEConnect.Modals.Task>>(Constants.ApiResponseType.Success, taskItems);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<SMEConnect.Modals.Task>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }

        public async Task<ApiResponse<SMEConnect.Modals.Task>> GetTaskItem(int id)
        {
            try
            {
                var taskItem = await _context.Tasks.Include(e => e.Employee).Where(e => e.Employee.Id == e.EmployeeId).FirstAsync();
                if (taskItem != null)
                {
                    return new ApiResponse<SMEConnect.Modals.Task>(Constants.ApiResponseType.Success, taskItem);
                }
                return new ApiResponse<SMEConnect.Modals.Task>(Constants.ApiResponseType.Success, null, "SMEConnect.Modals.Task not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<SMEConnect.Modals.Task>(Constants.ApiResponseType.Failure, null, ex.Message);
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
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, false, "SMEConnect.Modals.Task not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateTaskItem(SMEConnect.Modals.Task taskItem)
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

        public async Task<ApiResponse<bool>> UpdateTaskItem(SMEConnect.Modals.Task taskItem)
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
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "SMEConnect.Modals.Task not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }
    }

}
