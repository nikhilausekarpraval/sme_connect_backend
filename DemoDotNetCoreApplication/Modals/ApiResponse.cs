using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DemoDotNetCoreApplication.Modals
{
        public class ApiResponse<T>
        {
            public string Status { get; set; }

            public T? Data { get; set; }
            public string Message { get; set; } 

            public ApiResponse(string status, T? data, string message = null)
            {
                Status = status;
                Data  = data;
                Message = message;
            }
        }

}
