using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class GradeClassConfiguration : IEntityTypeConfiguration<GradeClass>
    {
        public void Configure(EntityTypeBuilder<GradeClass> builder)
        {
            builder.HasKey(gc => gc.Id);

            builder.HasOne(gc => gc.Grade)
                .WithMany(g => g.GradeClasses)
                .HasForeignKey(gc => gc.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gc => gc.Class)
                .WithMany(c => c.GradeClasses)
                .HasForeignKey(gc => gc.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(gc => new { gc.GradeId, gc.ClassId })
                .IsUnique();
        }
    }
}
