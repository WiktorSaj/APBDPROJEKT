using APBDPROJEKT.DTOs.RevenueDTOs;

namespace APBDPROJEKT.Services;

public interface IRevenueService
{
    Task<GetRevenueDetailsDto> CalculateRealizedRevenueAsync(int? softwareId, string? currency);
    Task<GetRevenueDetailsDto> CalculateForecastedRevenueAsync(int? softwareId, string? currency);
}