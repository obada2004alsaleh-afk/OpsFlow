

using Microsoft.EntityFrameworkCore;
using OpsFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpsFlow.Infrastructure.EntityConfigurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.TicketId);
            builder.Property(t => t.Title)
                   .IsRequired()
                   .HasMaxLength(200);
            builder.Property(t => t.Description)
                   .IsRequired()
                   .HasMaxLength(1000);
            builder.Property(t => t.Category)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(t => t.Priority)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.Property(t => t.Status)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(t => t.Site)
                   .WithMany(s => s.Tickets)
                   .HasForeignKey(t => t.SiteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Asset)
                   .WithMany(a => a.Tickets)
                   .HasForeignKey(t => t.AssetId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.CreatedByUser)
                   .WithMany(u => u.Tickets)
                   .HasForeignKey(t => t.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
