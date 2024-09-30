using AutoMapper;
using DemoDotNetCoreApplication.Constatns;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;

namespace DemoDotNetCoreApplication.Providers
{
    public class EmployeeProvider : IEmployeeProvider
    {
        private readonly DbContextProvider _context;


        public EmployeeProvider(DbContextProvider context)
        {
            _context = context;
            _context.Database.EnsureCreated();

        }

        public async Task<ApiResponse<List<Employee>>> getEmployees()
        {
           
            try
            {
                //var employees = await _context.Employees.Include(e => e.taskItems).Where(e => e.taskItems.Any(t => t.employeeId == e.Id)).ToListAsync();
                //return new ApiResponse<List<Employee>>(status:Constants.ApiResponseType.Success,data:employees,message:"");

                var employees = await _context.Employees
                                          .Include(e => e.task_items)
                                          .ToListAsync();

                return new ApiResponse<List<Employee>>(Constants.ApiResponseType.Success, employees);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Employee>>(status: Constants.ApiResponseType.Failure, data: null, message: ex.Message);
            }
        }

        public async Task<ApiResponse<Employee>> GetEmployee(int? id)
        {
            try
            {
                var employee = await _context.Employees.Include(e => e.task_items).Where(e => e.id == id).FirstAsync();
                if (employee != null)
                {
                    return new ApiResponse<Employee>(status: Constants.ApiResponseType.Success, data: employee, message: "");
                }
                return new ApiResponse<Employee>(status: Constants.ApiResponseType.Success, data: employee, message: "Employee not found");
            }
            catch (Exception ex)
            {
                return new ApiResponse<Employee>(status: Constants.ApiResponseType.Failure, data: null, message: ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChanges();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, false, "Employee not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> CreateEmployee(Employee employee)
        {
            try
            {
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateEmployee(Employee employee)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(employee.id);
                if (existingEmployee != null)
                {
                    _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
                    _context.SaveChanges();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "Employee not found.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }
    }

}
