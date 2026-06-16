using APBDPROJEKT.DTOs.ClientDTOs;

namespace APBDPROJEKT.Services;

public interface IClientService
{
    Task CreateIndividualClientAsync(CreateIndividualClientDto createIndividualClientDto);
    Task CreateCompanyClientAsync(CreateCompanyClientDto createCompanyClientDto);
    
    Task UpdateCompanyClientAsync(int id, UpdateCompanyClientDto updateCompanyClientDto);
    Task UpdateIndividualClientAsync(int id, UpdateIndividualClientDto updateIndividualClientDto);

    Task DeleteClientAsync(int id);
}