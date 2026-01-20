using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;
using SistepedApi.Models.Enums;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(u => u.Name);

            builder.Property(u => u.Role)
                .HasConversion<int>()
                .HasDefaultValue(UserRole.Guardian)
                .IsRequired();

            builder.Property(u => u.Status)
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasOne(u => u.Credential)
                .WithOne(uc => uc.User)
                .HasForeignKey<UserCredential>(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}