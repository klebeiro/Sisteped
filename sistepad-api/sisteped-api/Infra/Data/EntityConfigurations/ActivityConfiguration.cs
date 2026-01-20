using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Description)
                .HasMaxLength(1000);

            builder.Property(a => a.ApplicationDate)
                .IsRequired();

            builder.Property(a => a.MaxScore)
                .IsRequired()
                .HasPrecision(5, 2)
                .HasDefaultValue(10);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasOne(a => a.Grade)
                .WithMany(g => g.Activities)
                .HasForeignKey(a => a.GradeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.GradeId);
            builder.HasIndex(a => a.ApplicationDate);
        }
    }
}
