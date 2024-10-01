using AutoMapper;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Modals;

namespace DemoDotNetCoreApplication.Providers
{
    public class AutoMapperProvider : Profile
    {
        public AutoMapperProvider() {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<DemoDotNetCoreApplication.Modals.Task, TaskItemsDto>();
            CreateMap<TaskItemsDto, DemoDotNetCoreApplication.Modals.Task>();

            CreateMap<Employee,EmployeeTasksDto>();
            CreateMap<EmployeeTasksDto, Employee>();
            
        }
    }

}

