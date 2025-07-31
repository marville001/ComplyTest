using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComplyTest.Domain.Entities;

namespace ComplyTest.Infrastructure.Configuration;

public class EmployeeProjectConfiguration : IEntityTypeConfiguration<EmployeeProject>
{
    public void Configure(EntityTypeBuilder<EmployeeProject> builder)
    {
        builder.HasKey(ep => new { ep.EmployeeId, ep.ProjectId });
        builder.Property(ep => ep.Role).IsRequired();
            
        builder.HasOne(ep => ep.Employee)
            .WithMany(e => e.EmployeeProjects)
            .HasForeignKey(ep => ep.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);
            
        builder.HasOne(ep => ep.Project)
            .WithMany(p => p.EmployeeProjects)
            .HasForeignKey(ep => ep.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}