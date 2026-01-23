using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class GridClassConfiguration : IEntityTypeConfiguration<GridClass>
    {
        public void Configure(EntityTypeBuilder<GridClass> builder)
        {
            builder.ToTable("GridClasses");

            builder.HasKey(gc => gc.Id);

            builder.Property(gc => gc.Id)
                .ValueGeneratedOnAdd();

            builder.Property(gc => gc.GridId)
                .IsRequired();

            builder.Property(gc => gc.ClassId)
                .IsRequired();

            builder.Property(gc => gc.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')");

            builder.HasOne(gc => gc.Grid)
                .WithMany(g => g.GridClasses)
                .HasForeignKey(gc => gc.GridId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gc => gc.Class)
                .WithMany(c => c.GridClasses)
                .HasForeignKey(gc => gc.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(gc => new { gc.GridId, gc.ClassId })
                .IsUnique();
        }
    }
}
