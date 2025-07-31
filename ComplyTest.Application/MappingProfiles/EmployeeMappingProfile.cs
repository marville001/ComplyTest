using AutoMapper;
using ComplyTest.Application.DTOs;
using ComplyTest.Domain.Entities;

namespace ComplyTest.Application.MappingProfiles;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();
    }
}