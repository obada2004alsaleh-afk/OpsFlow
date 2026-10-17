


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.Configurations
{
    public class TechnicianProfileConfiguration : IEntityTypeConfiguration<TechnicianProfile>
    {

        public void Configure(EntityTypeBuilder<TechnicianProfile> builder)
        {
            builder.HasKey(s => s.TechnicalId);

            builder.Property(s => s.Specialization)
                   .IsRequired()
                   .HasMaxLength(150);



        }
    }
}