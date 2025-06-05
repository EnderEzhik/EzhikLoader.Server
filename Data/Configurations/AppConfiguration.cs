using EzhikLoader.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EzhikLoader.Server.Data.Configurations
{
    public class AppConfiguration : IEntityTypeConfiguration<App>
    {
        public void Configure(EntityTypeBuilder<App> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).HasMaxLength(32).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();

            entity.Property(e => e.Description).HasColumnType("TEXT").IsRequired();

            entity.Property(e => e.Version).IsRequired();

            entity.Property(e => e.Price).HasColumnType("DECIMAL(6, 2)").IsRequired();

            entity.Property(e => e.IsActive).IsRequired();

            entity.Property(e => e.CreatedAt).HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP()");

            entity.Property(e => e.LastUpdatedAt).HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP()");

            entity.Property(e => e.FileName).HasMaxLength(32).IsRequired();
            entity.HasIndex(e => e.FileName).IsUnique();

            entity.Property(e => e.IconName).HasMaxLength(32);
            entity.HasIndex(e => e.IconName).IsUnique();
        }
    }
}
