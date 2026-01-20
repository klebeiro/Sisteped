using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Date)
                .IsRequired();

            builder.Property(a => a.Present)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Grade)
                .WithMany()
                .HasForeignKey(a => a.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => new { a.StudentId, a.GradeId, a.Date })
                .IsUnique();
        }
    }
}
