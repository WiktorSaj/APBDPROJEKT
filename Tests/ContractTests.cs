using APBDPROJEKT.Data;
using APBDPROJEKT.DTOs.ContractDTOs;
using APBDPROJEKT.Entities;
using APBDPROJEKT.Exceptions;
using APBDPROJEKT.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class ContractTests
{
    private readonly RevenueDbContext _context;
    private readonly ContractService _service;
    
    public ContractTests()
    {
        var opt = new DbContextOptionsBuilder<RevenueDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new RevenueDbContext(opt);
        _service = new ContractService(_context);
    }

    [Fact]
    public async Task CreateContractAsync_WhenDaysNotBetween3and30()
    {
        var fakeContract = new CreateContractDto
        {
            ClientId = 1,
            SoftwareId = 1,
            PaymentDeadlineDays = 2,
            BonusSupportYears = 0
        };
        
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateContractAsync(fakeContract));
        Assert.Equal("Payment period must be between 3 and 30 days", exception.Message);
    }

    [Fact]
    public async Task CreateContractAsync_WhenBonusSupportYearsProvided()
    {
        int clientId = 1;
        int softwareId = 1;

        var client = new IndividualClient
        {
            Id = clientId,
            FirstName = "John",
            LastName = "Doe",
            Email = "email@email.com",
            PhoneNumber = "111222333",
            IsDeleted = false,
            Address = "Address"
        };

        var soft = new Software
        {
            Id = softwareId,
            Name = "Software",
            BasePricePerYear = 5000m,
            Version = "2.0.0"
        };
        await _context.Clients.AddAsync(client);
        await _context.Software.AddAsync(soft);
        await _context.SaveChangesAsync();

        var fakeContract = new CreateContractDto
        {
            ClientId = clientId,
            SoftwareId = softwareId,
            PaymentDeadlineDays = 15,
            BonusSupportYears = 2,
        };
        
        await _service.CreateContractAsync(fakeContract);
        
        var created =  await _context.Contracts.FirstOrDefaultAsync(c => c.Id == clientId);
        
        Assert.NotNull(created);
        Assert.Equal(7000m, created.Price);
    }
}