using EzhikLoader.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EzhikLoader.Server.Data.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.IsCanceled).HasDefaultValue(false);

            entity.Property(e => e.UserId).IsRequired();

            entity.Property(e => e.AppId).IsRequired();

            entity.Property(e => e.StartDate).HasColumnType("TIMESTAMP").IsRequired();

            entity.Property(e => e.EndDate).HasColumnType("TIMESTAMP").IsRequired();

            entity.Property(e => e.LastDownloadedAt).HasColumnType("TIMESTAMP");
        }
    }
}
