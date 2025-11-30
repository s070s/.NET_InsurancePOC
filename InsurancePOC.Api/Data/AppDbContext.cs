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
    }
}


