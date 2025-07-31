using ComplyTest.Application.DTOs;

namespace ComplyTest.Application.Mappers.Project;

public interface IProjectMapper
{
    ProjectDto ToDto(ComplyTest.Domain.Entities.Project project);
    ComplyTest.Domain.Entities.Project ToDomain(ProjectDto projectDto);
}