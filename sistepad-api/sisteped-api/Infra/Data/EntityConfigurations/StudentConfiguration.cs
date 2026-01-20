using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Enrollment)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(s => s.Enrollment)
                .IsUnique();

            builder.Property(s => s.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(s => s.BirthDate)
                .IsRequired();

            builder.Property(s => s.Status)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasOne(s => s.Guardian)
                .WithMany()
                .HasForeignKey(s => s.GuardianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.Name);
        }
    }
}
