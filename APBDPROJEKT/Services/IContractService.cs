using APBDPROJEKT.DTOs.ContractDTOs;

namespace APBDPROJEKT.Services;

public interface IContractService
{
    Task CreateContractAsync(CreateContractDto dto);
}