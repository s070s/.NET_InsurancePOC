using Microsoft.EntityFrameworkCore;
using InsurancePOC.Api.Models;

namespace InsurancePOC.Api.Data
{
    /// <summary>
    /// Database context for the Insurance POC application.
    /// Manages Client and Policy entities and their relationships.
    /// </summary>
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        
        // DbSet properties expose tables for querying and saving data
        public DbSet<Policy> Policies => Set<Policy>();
        public DbSet<Client> Clients => Set<Client>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for PremiumAmount
            // 18 total digits, 2 after decimal point (e.g., 9999999999999999.99)
            modelBuilder.Entity<Policy>()
                .Property(p => p.PremiumAmount)
                .HasPrecision(18, 2);
        }
    }
}


