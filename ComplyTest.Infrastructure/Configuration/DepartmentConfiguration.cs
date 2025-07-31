using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComplyTest.Domain.Entities;

namespace ComplyTest.Infrastructure.Configuration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired();
        builder.Property(d => d.OfficeLocation).IsRequired();
            
        builder.HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
            
        builder.HasMany(d => d.Projects)
            .WithOne(p => p.Department)
            .HasForeignKey(p => p.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}