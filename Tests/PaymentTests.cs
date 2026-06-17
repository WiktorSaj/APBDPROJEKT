using APBDPROJEKT.Data;
using APBDPROJEKT.DTOs.PaymentDTOs;
using APBDPROJEKT.Entities;
using APBDPROJEKT.Exceptions;
using APBDPROJEKT.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class PaymentTests
{
    private readonly RevenueDbContext _context;
    private readonly PaymentService _service;
    
    public PaymentTests()
    {
        var opt = new DbContextOptionsBuilder<RevenueDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new RevenueDbContext(opt);
        _service = new PaymentService(_context);
    }

    [Fact]
    public async Task CreatePaymentAsync_WhenContractHasExpired()
    {
        int contractId = 1;

        var expired = new Contract
        {
            Id = contractId,
            ClientId = 1,
            SoftwareId = 1,
            Price = 5000m,
            IsSigned = false,
            EndDate = DateTime.Now.AddDays(-1)
        };
        
        await _context.Contracts.AddAsync(expired);
        await _context.SaveChangesAsync();

        var fakePayment = new CreatePaymentDto
        {
            ContractId = contractId,
            ClientId = 1,
            Amount = 5000m,
        };

        var exception = await Assert.ThrowsAnyAsync<BadRequestException>(() =>
            _service.CreatePaymentAsync(fakePayment)
        );
        
        Assert.Equal("The payment deadline for this contract has expired", exception.Message);
    }

    [Fact]
    public async Task CreatePaymentAsync_WhenTotalIsMetContractIsSigned()
    {
        int contractId = 1;
        var contract = new Contract
        {
            Id = contractId,
            ClientId = 1,
            SoftwareId = 1,
            Price = 5000m,
            IsSigned = false,
            EndDate = DateTime.Now.AddDays(7)
        };

        var fakePayment = new CreatePaymentDto
        {
            ContractId = contractId,
            ClientId = 1,
            Amount = 5000m,
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        
        await _service.CreatePaymentAsync(fakePayment);
        var signedContract = await _context.Contracts.FirstOrDefaultAsync(c => c.Id == contract.Id);
        
        Assert.NotNull(signedContract);
        Assert.True(signedContract.IsSigned);
    }
}