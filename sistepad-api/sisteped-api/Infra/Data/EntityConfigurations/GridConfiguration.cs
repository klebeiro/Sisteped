using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class GridConfiguration : IEntityTypeConfiguration<Grid>
    {
        public void Configure(EntityTypeBuilder<Grid> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Year)
                .IsRequired();

            builder.Property(g => g.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(g => g.Status)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(g => new { g.Year, g.Name })
                .IsUnique();

            builder.HasMany(g => g.Grades)
                .WithOne(gr => gr.Grid)
                .HasForeignKey(gr => gr.GridId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
