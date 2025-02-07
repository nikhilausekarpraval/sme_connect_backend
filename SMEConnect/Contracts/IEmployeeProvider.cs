using SMEConnect.Modals;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SMEConnect.Contracts
{
    public interface IEmployeeProvider
    {
        public  Task<ApiResponse<List<Employee>>> getEmployees();

        public  Task<ApiResponse<Employee>> GetEmployee(int? id);

        public  Task<ApiResponse<bool>> DeleteEmployee(int id);

        public  Task<ApiResponse<bool>> CreateEmployee(Employee employee);

        public  Task<ApiResponse<bool>> UpdateEmployee(Employee employee);

    }
}
