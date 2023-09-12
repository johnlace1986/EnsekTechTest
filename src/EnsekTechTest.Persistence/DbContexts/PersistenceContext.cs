using EnsekTechTest.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EnsekTechTest.Persistence.DbContexts
{
    internal class PersistenceContext : DbContext
    {
        public PersistenceContext() : base()
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured is false)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("persistencesettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(
                    connectionString: configuration.GetSection("sql:connectionString").Value);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().HasData(new Account { Id = 2344, FirstName = "Tommy", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2233, FirstName = "Barry", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 8766, FirstName = "Sally", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2345, FirstName = "Jerry", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2346, FirstName = "Ollie", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2347, FirstName = "Tara", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2348, FirstName = "Tammy", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2349, FirstName = "Simon", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2350, FirstName = "Colin", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2351, FirstName = "Gladys", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2352, FirstName = "Greg", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2353, FirstName = "Tony", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2355, FirstName = "Arthur", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 2356, FirstName = "Craig", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 6776, FirstName = "Laura", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 4534, FirstName = "Josh", LastName = "TEST", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1234, FirstName = "Freya", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1239, FirstName = "Noddy", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1240, FirstName = "Archie", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1241, FirstName = "Lara", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1242, FirstName = "Tim", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1243, FirstName = "Graham", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1244, FirstName = "Tony", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1245, FirstName = "Neville", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1246, FirstName = "Jo", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1247, FirstName = "Jim", LastName = "Test", });
            modelBuilder.Entity<Account>().HasData(new Account { Id = 1248, FirstName = "Pam", LastName = "Test", });
        }
    }
}
