using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(g => g.Level)
                .IsRequired();

            builder.Property(g => g.Shift)
                .IsRequired();

            builder.Property(g => g.Status)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(g => g.Name);
        }
    }
}
