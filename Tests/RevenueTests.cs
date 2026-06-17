using APBDPROJEKT.Data;
using APBDPROJEKT.Entities;
using APBDPROJEKT.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class RevenueTests
{
    private readonly RevenueDbContext _context;
    private readonly RevenueService _service;
    
    public RevenueTests()
    {
        var opt = new DbContextOptionsBuilder<RevenueDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new RevenueDbContext(opt);
        _service = new RevenueService(_context, null);
        
    }

    [Fact]
    public async Task CalculateRealizedRevenueAsync_WithSofwareId()
    {
        

        var soft1 = new Software
        {
            Id = 1,
            Name = "Some Software",
        };
        var soft2 = new Software
        {
            Id = 2,
            Name = "Some Software2",
        };
        
        await _context.Software.AddRangeAsync(soft1, soft2);
        await _context.SaveChangesAsync();
        
        await _context.Contracts.AddRangeAsync(
            new Contract { Id = 1, SoftwareId = 1, Price = 5000m, IsSigned = true },
            new Contract { Id = 2, SoftwareId = 1, Price = 3000m, IsSigned = true },
            new Contract { Id = 3, SoftwareId = 2, Price = 10000m, IsSigned = true },
            new Contract { Id = 4, SoftwareId = 1, Price = 4500m, IsSigned = false } 
        );
        await _context.SaveChangesAsync();
        
        var result = await _service.CalculateRealizedRevenueAsync(1, null);
        Assert.Equal(8000m, result.TotalAmount);
    }

    [Fact]
    public async Task CalculateForecastedRevenueAsync_WithoutSofwareId()
    {
        await _context.Contracts.AddRangeAsync(
            new Contract { Id = 10, SoftwareId = 1, Price = 4000m, IsSigned = true, EndDate = DateTime.Now.AddDays(5) },
            new Contract { Id = 11, SoftwareId = 2, Price = 6000m, IsSigned = false, EndDate = DateTime.Now.AddDays(5) }, 
            new Contract { Id = 12, SoftwareId = 3, Price = 3000m, IsSigned = false, EndDate = DateTime.Now.AddDays(-1) } 
        );
        await _context.SaveChangesAsync();
        
        var result = await _service.CalculateForecastedRevenueAsync(null, null);
        Assert.Equal(10000m, result.TotalAmount);
    }
}