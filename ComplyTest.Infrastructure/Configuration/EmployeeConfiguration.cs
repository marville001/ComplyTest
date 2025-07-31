using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComplyTest.Domain.Entities;

namespace ComplyTest.Infrastructure.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.FirstName).IsRequired();
        builder.Property(e => e.LastName).IsRequired();
        builder.Property(e => e.Email).IsRequired();
        builder.Property(e => e.Salary).HasPrecision(18, 2);
            
        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);
            
        builder.HasMany(e => e.EmployeeProjects)
            .WithOne(ep => ep.Employee)
            .HasForeignKey(ep => ep.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}