using AutoMapper;
using ComplyTest.Application.DTOs;
using ComplyTest.Domain.Entities;

namespace ComplyTest.Application.MappingProfiles;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();
    }
}