using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class GridGradeConfiguration : IEntityTypeConfiguration<GridGrade>
    {
        public void Configure(EntityTypeBuilder<GridGrade> builder)
        {
            builder.ToTable("GridGrades");

            builder.HasKey(gg => gg.Id);

            builder.Property(gg => gg.Id)
                .ValueGeneratedOnAdd();

            builder.Property(gg => gg.GridId)
                .IsRequired();

            builder.Property(gg => gg.GradeId)
                .IsRequired();

            builder.Property(gg => gg.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')");

            builder.HasOne(gg => gg.Grid)
                .WithMany(g => g.GridGrades)
                .HasForeignKey(gg => gg.GridId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gg => gg.Grade)
                .WithMany(gr => gr.GridGrades)
                .HasForeignKey(gg => gg.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(gg => new { gg.GridId, gg.GradeId })
                .IsUnique();
        }
    }
}
