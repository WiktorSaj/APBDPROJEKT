using APBDPROJEKT.Entities;
using Microsoft.EntityFrameworkCore;

namespace APBDPROJEKT.Data;

public class RevenueDbContext : DbContext
{
    protected RevenueDbContext()
    {
    }

    public RevenueDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Client> Clients { get; set; }
    public DbSet<IndividualClient>  IndividualClients { get; set; }
    public DbSet<CompanyClient>  CompanyClients { get; set; }
    public DbSet<Software>  Software { get; set; }
    public DbSet<Discount>  Discounts { get; set; }
    public DbSet<Contract>  Contracts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<AppUser>  AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.Entity<IndividualClient>().HasData(new IndividualClient
            {
                Id = 1,
                Address = "ul. Pierwsza 1, 00-001 Warszawa",
                Email = "first.user@poczta.com",
                FirstName = "John",
                LastName = "Doe",
                Pesel = "12345678901",
                PhoneNumber = "123456789",
                IsDeleted = false,
            },
            new IndividualClient()
            {
                Id = 2,
                Address = "ul. Druga 2, 30-000 Kraków",
                Email = "second.user@poczta.com",
                FirstName = "Adam",
                LastName = "Smith",
                Pesel = "23456789012",
                PhoneNumber = "987654321",
                IsDeleted = false,
            });

        modelBuilder.Entity<CompanyClient>().HasData(new CompanyClient
            {
                Id = 3,
                Address = "ul. Trzecia 3, 00-001 Warszawa",
                Email = "third.user@poczta.com",
                PhoneNumber = "123654789",
                CompanyName = "CorporationFirst",
                Krs = "0000123456"
            },
            new CompanyClient()
            {
                Id = 4,
                Address = "ul. Czwarta 4, 30-000 Kraków",
                Email = "fourth.user@poczta.com",
                PhoneNumber = "321456987",
                CompanyName = "CorporationSecond",
                Krs = "0000234567"
            });

        modelBuilder.Entity<Software>().HasData(new Software
            {
                Id = 1,
                Name = "Software 1",
                Description = "Software Description 1",
                Version = "1.0.0",
                Category = "Education",
                BasePricePerYear = 2500.00m
            },
            new Software()
            {
                Id = 2,
                Name = "Software 2",
                Description = "Software Description 2",
                Version = "2.0.0",
                Category = "Finance",
                BasePricePerYear = 5000.00m
            }
        );

        modelBuilder.Entity<Discount>().HasData(new Discount
            {
                Id = 1,
                Name = "Discount 1",
                Value = 10.00m,
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 3, 31),
                SoftwareId = 1
            },
            new Discount()
            {
                Id = 2,
                Name = "Discount 2",
                Value = 20.00m,
                StartDate = new DateTime(2026, 3, 1),
                EndDate = new DateTime(2026, 8, 31),
                SoftwareId = 2
            }
        );

        modelBuilder.Entity<Contract>().HasData(new Contract
            {
                Id = 1,
                SoftwareVersion = "1.0.0",
                StartDate = new DateTime(2026, 6, 1),
                EndDate = new DateTime(2026, 6, 30),
                Price = 4500.00m,
                BonusSupportYears = 0,
                IsSigned = true,
                ClientId = 1,
                SoftwareId = 1
            },
            new Contract()
            {
                Id = 2,
                SoftwareVersion = "2.0.0",
                StartDate = new DateTime(2026, 6, 12),
                EndDate = new DateTime(2026, 6, 26),
                Price = 3000.00m,
                BonusSupportYears = 1,
                IsSigned = false,
                ClientId = 3,
                SoftwareId = 2
            }
        );

        modelBuilder.Entity<Payment>().HasData(
            new Payment
            {
                Id = 1,
                Amount = 2000.00m,
                Date = new DateTime(2026, 6, 2),
                ContractId = 1
            },
            new Payment()
            {
                Id = 2,
                Amount = 2500.00m,
                Date = new DateTime(2026, 6, 5),
                ContractId = 1
            }
        );

        modelBuilder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = 1,
                Login = "appUser1",
                Password = "password1",
                Role = "Admin",
            },
            new AppUser
            {
                Id = 2,
                Login = "appUser2",
                Password = "password2",
                Role = "User",
            }
        );
    }
}