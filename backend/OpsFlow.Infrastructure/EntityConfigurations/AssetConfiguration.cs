
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.Configurations
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
         
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(a => a.AssetId);
         
            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasOne(a => a.Site)
        .WithMany(s => s.Assets)
        .HasForeignKey(a => a.SiteId)
        .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
