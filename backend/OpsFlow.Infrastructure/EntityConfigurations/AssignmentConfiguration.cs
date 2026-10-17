


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.Configurations
{
    public class AssignmentConfiguration :IEntityTypeConfiguration<Assignment>
    {


        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.AssignmentId);



            builder.HasOne(a => a.AssignedByUser)
            .WithMany(s => s.Assignments)
            .HasForeignKey(a => a.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(u => u.TicketId)
                .IsUnique()
                .HasFilter("[IsActive] = 1");


            builder.HasOne(t=>t.TechnicianProfile)
                .WithMany(t=>t.Assignments)
                .HasForeignKey(a => a.TechnicalId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(t => t.Ticket)
                .WithMany(t => t.Assignments)
                .HasForeignKey(a => a.TicketId)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
