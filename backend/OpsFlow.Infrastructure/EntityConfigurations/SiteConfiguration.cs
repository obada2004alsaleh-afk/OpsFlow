using Microsoft.EntityFrameworkCore;
using OpsFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace OpsFlow.Infrastructure.Configurations
{
    public class SiteConfiguration : IEntityTypeConfiguration<Site>
    {

        public void Configure(EntityTypeBuilder<Site> builder)
        {
            builder.HasKey(s => s.SiteId);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(s => s.Email)
                   .HasMaxLength(255);

            builder.Property(s => s.Phone)
                   .HasMaxLength(20);

            builder.Property(s => s.Address)
                   .IsRequired()
                   .HasMaxLength(300);


            builder.HasOne(s => s.Customer)
       .WithMany(c => c.Sites)
       .HasForeignKey(s => s.CustomerId)
       .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
