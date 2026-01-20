using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class StudentGradeConfiguration : IEntityTypeConfiguration<StudentGrade>
    {
        public void Configure(EntityTypeBuilder<StudentGrade> builder)
        {
            builder.HasKey(sg => sg.Id);

            builder.HasOne(sg => sg.Student)
                .WithMany(s => s.StudentGrades)
                .HasForeignKey(sg => sg.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sg => sg.Grade)
                .WithMany()
                .HasForeignKey(sg => sg.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(sg => new { sg.StudentId, sg.GradeId })
                .IsUnique();
        }
    }
}
