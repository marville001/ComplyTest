using ComplyTest.Application.DTOs;
using AutoMapper;

namespace ComplyTest.Application.Mappers.Project;

public class ProjectMapper : IProjectMapper
{
    private readonly IMapper _mapper;

    public ProjectMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ProjectDto ToDto(ComplyTest.Domain.Entities.Project project)
        => _mapper.Map<ProjectDto>(project);

    public ComplyTest.Domain.Entities.Project ToDomain(ProjectDto projectDto)
        => _mapper.Map<ComplyTest.Domain.Entities.Project>(projectDto);
}