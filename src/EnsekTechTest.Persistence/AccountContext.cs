using EnsekTechTest.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EnsekTechTest.Persistence
{
    public class AccountContext : DbContext
    {
        public AccountContext() : base()
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured is false)
            {
                optionsBuilder.UseSqlServer(
                    connectionString: "Server=(LocalDb)\\MSSQLLocalDB;Database=EnsekTechTest;Trusted_Connection=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
