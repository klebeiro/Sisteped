using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistepedApi.Models;
using SistepedApi.Models.Enums;

namespace SistepedApi.Infra.Data.EntityConfigurations
{
    public class UserCredentialConfiguration : IEntityTypeConfiguration<UserCredential>
    {
        public void Configure(EntityTypeBuilder<UserCredential> builder)
        {
            builder.HasKey(uc => uc.UserId);

            builder.Property(uc => uc.PasswordHash)
                .IsRequired();

            builder.Property(uc => uc.Role)
                .HasConversion<int>()
                .HasDefaultValue(UserRole.Guardian)
                .IsRequired();
        }
    }
}