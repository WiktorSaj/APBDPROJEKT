using APBDPROJEKT.Data;
using APBDPROJEKT.DTOs.ClientDTOs;
using APBDPROJEKT.Entities;
using APBDPROJEKT.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBDPROJEKT.Services;

public class ClientService : IClientService
{
    private readonly RevenueDbContext _dbContext;
    
    public  ClientService(RevenueDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateIndividualClientAsync(CreateIndividualClientDto createIndividualClientDto)
    {
        var peselExists = await _dbContext.IndividualClients.AnyAsync(c => c.Pesel == createIndividualClientDto.Pesel);
        if (peselExists)
        {
            throw new ConflictException("Client with given pesel already exists");
        }

        var client = new IndividualClient()
        {
            FirstName = createIndividualClientDto.FirstName,
            LastName = createIndividualClientDto.LastName,
            Address = createIndividualClientDto.Address,
            Email = createIndividualClientDto.Email,
            PhoneNumber = createIndividualClientDto.PhoneNumber,
            Pesel = createIndividualClientDto.Pesel,
        };
        
        await _dbContext.IndividualClients.AddAsync(client);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateCompanyClientAsync(CreateCompanyClientDto createCompanyClientDto)
    {
        var krsExists = await _dbContext.CompanyClients.AnyAsync(c => c.Krs == createCompanyClientDto.Krs);
        if (krsExists)
        {
            throw new ConflictException("Client with given krs already exists");
        }

        var client = new CompanyClient()
        {
            CompanyName = createCompanyClientDto.CompanyName,
            Address = createCompanyClientDto.Address,
            Email = createCompanyClientDto.Email,
            PhoneNumber = createCompanyClientDto.PhoneNumber,
            Krs = createCompanyClientDto.Krs,
        };
        await _dbContext.CompanyClients.AddAsync(client);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCompanyClientAsync(int id, UpdateCompanyClientDto updateCompanyClientDto)
    {
        var client = await _dbContext.CompanyClients.FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
        {
            throw new NotFoundException("Client with given id does not exist");
        }
        client.CompanyName = updateCompanyClientDto.CompanyName;
        client.Address = updateCompanyClientDto.Address;
        client.Email = updateCompanyClientDto.Email;
        client.PhoneNumber = updateCompanyClientDto.PhoneNumber;
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateIndividualClientAsync(int id, UpdateIndividualClientDto updateIndividualClientDto)
    {
        var client = await _dbContext.IndividualClients.FirstOrDefaultAsync(c => c.Id == id);
        if (client == null || client.IsDeleted)
        {
            throw new NotFoundException("Client with given id does not exist");
        }
        client.FirstName = updateIndividualClientDto.FirstName;
        client.LastName = updateIndividualClientDto.LastName;
        client.Address = updateIndividualClientDto.Address;
        client.PhoneNumber = updateIndividualClientDto.PhoneNumber;
        client.Email = updateIndividualClientDto.Email;
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteClientAsync(int id)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
        {
            throw new NotFoundException("Client with given id does not exist");
        }

        if (client is CompanyClient)
        {
            throw new BadRequestException("Company clients cannot be deleted");
        }

        if (client is IndividualClient individualClient)
        {
            if(individualClient.IsDeleted)
                throw new NotFoundException("This client has already been deleted");
            individualClient.IsDeleted = true;
        }
        
        await _dbContext.SaveChangesAsync();
    }
}