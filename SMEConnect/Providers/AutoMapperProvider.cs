using AutoMapper;
using SMEConnect.Dtos;
using SMEConnect.Modals;

namespace SMEConnect.Providers
{
    public class AutoMapperProvider : Profile
    {
        public AutoMapperProvider() {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<SMEConnect.Modals.Task, TaskItemsDto>();
            CreateMap<TaskItemsDto, SMEConnect.Modals.Task>();

            CreateMap<Employee,EmployeeTasksDto>();
            CreateMap<EmployeeTasksDto, Employee>();

            CreateMap<string, RoleDto>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src)) 
             .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }

}

