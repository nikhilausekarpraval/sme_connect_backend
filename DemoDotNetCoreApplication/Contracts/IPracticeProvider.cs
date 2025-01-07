using DemoDotNetCoreApplication.Modals;
using Microsoft.EntityFrameworkCore;

namespace DemoDotNetCoreApplication.Contracts
{
    public interface IPracticeProvider
    {
        public  Task<ApiResponse<List<Practice>>> getPractices();


        public  Task<ApiResponse<bool>> DeletePractice(List<int> practices);


        public  Task<ApiResponse<bool>> CreatePractice(Practice Practice);

        public Task<ApiResponse<bool>> UpdatePractice(Practice practice);

    }
}
