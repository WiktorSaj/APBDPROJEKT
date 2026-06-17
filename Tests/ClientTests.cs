using APBDPROJEKT.Data;
using APBDPROJEKT.DTOs.ClientDTOs;
using APBDPROJEKT.Entities;
using APBDPROJEKT.Exceptions;
using APBDPROJEKT.Services;
using Microsoft.EntityFrameworkCore;


namespace Tests;

public class ClientTests
{
    private readonly RevenueDbContext _context;
    private readonly ClientService _service;

    public ClientTests()
    {
        var opt = new DbContextOptionsBuilder<RevenueDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _context = new RevenueDbContext(opt);
        _service = new ClientService(_context);
    }
    
    [Fact]
    public async Task DeleteClientAsync_WhenClientIsCompanyClient()
    {
        var fakeClientId = 999;
        var fakeClient = new CompanyClient
        {
            Id = fakeClientId,
            CompanyName = "FakeABC"
        };
        
        await _context.Clients.AddAsync(fakeClient);
        await _context.SaveChangesAsync();
        
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.DeleteClientAsync(fakeClientId));
        
        Assert.Equal("Company clients cannot be deleted", exception.Message);
        
    }
    
    [Fact]
    public async Task CreateIndividualClientAsync_WhenPeselIsTaken()
    {
        var pesel = "11111111111";

        var firstFakeClient = new IndividualClient
        {
            Id = 1,
            FirstName = "FirstName",
            LastName = "LastName",
            Address = "Address",
            Email = "email@email.com",
            PhoneNumber = "987654321",
            IsDeleted = false,
            Pesel = pesel
        };
        
        await _context.Clients.AddAsync(firstFakeClient);
        await _context.SaveChangesAsync();

        var secondFakeClient = new CreateIndividualClientDto()
        {
            FirstName = "Joe",
            LastName = "Doe",
            Pesel = pesel,
            PhoneNumber = "123456789",
            Email = "email@email.com",
            Address = "Address",
            
        };
        
        var exception = await Assert.ThrowsAsync<ConflictException>(() => _service.CreateIndividualClientAsync(secondFakeClient));
        Assert.Equal("Client with given pesel already exists", exception.Message);
    }
}