using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasIndex(c => c.Email)
            .IsUnique();

            builder.Property(c => c.Email)
           .IsRequired()
           .HasMaxLength(255);

            builder.Property(c => c.Phone)
           .HasMaxLength(20);

         

        }
    }
}
