
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.EntityConfigurations
{
    public class TicketStatusHistoryConfiguration: IEntityTypeConfiguration<TicketStatusHistory>
    {

        public void Configure(EntityTypeBuilder<TicketStatusHistory> builder)
        {
            builder.HasKey(t => t.TicketStatusHistoryId);

            builder.Property(t => t.FromStatus)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(t => t.ToStatus)
                   .IsRequired()
                   .HasMaxLength(200);
            builder.Property(t => t.Reason)
                   .HasMaxLength(500);


            builder.HasOne(t => t.Ticket)
                   .WithMany(s => s.TicketStatusHistories)
                   .HasForeignKey(t => t.TicketId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(t => t.ChangedByUser)
                .WithMany(s => s.TicketStatusHistories)
                .HasForeignKey(t => t.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
