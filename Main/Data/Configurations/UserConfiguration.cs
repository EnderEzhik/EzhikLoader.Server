using EzhikLoader.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EzhikLoader.Server.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Login).HasMaxLength(20).IsRequired();
            entity.HasIndex(entity => entity.Login).IsUnique();

            entity.Property(e => e.Password).HasMaxLength(32).IsRequired();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.HasIndex(entity => entity.Email).IsUnique();

            entity.Property(e => e.RoleId).IsRequired();

            //entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.Property(e => e.RegistrationDate).HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP()");

            entity.Property(e => e.LastLoginDate).HasColumnType("TIMESTAMP");
        }
    }
}
