using Microsoft.EntityFrameworkCore;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Configurations;

namespace OpsFlow.Infrastructure.Database
{
    public class OpsFlowDbContext : DbContext
    {
        public OpsFlowDbContext(DbContextOptions<OpsFlowDbContext> options)
         : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TechnicianProfile> TechnicianProfiles { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new SiteConfiguration());
            modelBuilder.ApplyConfiguration(new AssetConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new TechnicianProfileConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
        }
    }
}
