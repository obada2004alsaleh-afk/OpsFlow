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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
               
        }
    }
}
