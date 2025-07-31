using AutoMapper;
using ComplyTest.Application.DTOs;
using ComplyTest.Domain.Entities;

namespace ComplyTest.Application.MappingProfiles;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
    }
}