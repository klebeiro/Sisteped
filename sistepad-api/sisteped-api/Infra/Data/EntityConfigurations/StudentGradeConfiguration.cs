using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class StudentGradeConfiguration : IEntityTypeConfiguration<StudentGrade>
    {
        public void Configure(EntityTypeBuilder<StudentGrade> builder)
        {
            builder.ToTable("StudentGrades");

            builder.HasKey(sg => sg.Id);

            builder.Property(sg => sg.Id)
                .ValueGeneratedOnAdd();

            builder.Property(sg => sg.StudentId)
                .IsRequired();

            builder.Property(sg => sg.GradeId)
                .IsRequired();

            builder.Property(sg => sg.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')");

            builder.HasOne(sg => sg.Student)
                .WithMany(s => s.StudentGrades)
                .HasForeignKey(sg => sg.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sg => sg.Grade)
                .WithMany(g => g.StudentGrades)
                .HasForeignKey(sg => sg.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(sg => new { sg.StudentId, sg.GradeId })
                .IsUnique();
        }
    }
}
