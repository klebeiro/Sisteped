using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class StudentActivityConfiguration : IEntityTypeConfiguration<StudentActivity>
    {
        public void Configure(EntityTypeBuilder<StudentActivity> builder)
        {
            builder.HasKey(sa => sa.Id);

            builder.Property(sa => sa.Score)
                .HasPrecision(5, 2);

            builder.Property(sa => sa.Remarks)
                .HasMaxLength(500);

            builder.HasOne(sa => sa.Student)
                .WithMany(s => s.StudentActivities)
                .HasForeignKey(sa => sa.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sa => sa.Activity)
                .WithMany(a => a.StudentActivities)
                .HasForeignKey(sa => sa.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(sa => new { sa.StudentId, sa.ActivityId })
                .IsUnique();

            builder.HasIndex(sa => sa.ActivityId);
        }
    }
}
