using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(c => c.Code)
                .IsUnique();

            builder.Property(c => c.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.WorkloadHours)
                .IsRequired();

            builder.Property(c => c.Status)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(c => c.Name);
        }
    }
}
