using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComplyTest.Domain.Entities;

namespace ComplyTest.Infrastructure.Configuration;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.ProjectCode).IsRequired();
        builder.Property(p => p.Budget).HasPrecision(18, 2);
            
        builder.HasOne(p => p.Department)
            .WithMany(d => d.Projects)
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
            
        builder.HasMany(p => p.EmployeeProjects)
            .WithOne(ep => ep.Project)
            .HasForeignKey(ep => ep.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}