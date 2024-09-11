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

            CreateMap<TaskItem, TaskItemsDto>();
            CreateMap<TaskItemsDto, TaskItem>();

            CreateMap<Employee,EmployeeTasksDto>();
            CreateMap<EmployeeTasksDto, Employee>();
            
        }
    }

}

