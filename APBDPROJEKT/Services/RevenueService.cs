using APBDPROJEKT.Data;
using APBDPROJEKT.DTOs.RevenueDTOs;
using APBDPROJEKT.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBDPROJEKT.Services;

public class RevenueService : IRevenueService
{
    private readonly RevenueDbContext _dbContext;
    private readonly HttpClient _httpClient;
    
    public RevenueService(RevenueDbContext dbContext,  HttpClient httpClient)
    {
        _dbContext = dbContext;
        _httpClient = httpClient;
    }

    public async Task<GetRevenueDetailsDto> CalculateRealizedRevenueAsync(int? softwareId, string? currency)
    {
        var contracts = _dbContext.Contracts.Where(c => c.IsSigned);
        if (softwareId.HasValue)
        {
            var softExists = await _dbContext.Software.AnyAsync(s => s.Id == softwareId.Value);
            if (!softExists)
            {
                throw new NotFoundException("Software with given id does not exist");
            }
            contracts = contracts.Where(c => c.SoftwareId == softwareId.Value);
        }
        
        decimal totalPln = await contracts.SumAsync(c => c.Price);
        
        return await ChangeCurrencyAsync(totalPln, currency);
    }

    public async Task<GetRevenueDetailsDto> CalculateForecastedRevenueAsync(int? softwareId, string? currency)
    {
        var contracts = _dbContext.Contracts.AsQueryable();
        if (softwareId.HasValue)
        {
            var softExists = await _dbContext.Software.AnyAsync(s => s.Id == softwareId.Value);
            if (!softExists)
            {
                throw new NotFoundException("Software with given id does not exist");
            }
            contracts = contracts.Where(c => c.SoftwareId == softwareId.Value);
        }
        
        decimal totalPln = await contracts.Where(c => c.IsSigned || c.EndDate >= DateTime.Now).SumAsync(c => c.Price);
        return await ChangeCurrencyAsync(totalPln, currency);
    }


    private async Task<GetRevenueDetailsDto> ChangeCurrencyAsync(decimal totalPln, string? currency)
    {
        decimal res = totalPln;
        string cleanCurrency = currency?.ToUpper() ?? "PLN";
        if (cleanCurrency != "PLN")
        {
            decimal rate = 0;
            string linkA = "http://api.nbp.pl/api/exchangerates/rates/a/" + cleanCurrency + "?format=json";
            string linkB = "http://api.nbp.pl/api/exchangerates/rates/b/" + cleanCurrency + "?format=json";

            try
            {
                var nbpRes = await _httpClient.GetFromJsonAsync<NbpResponse>(linkA);
                if (nbpRes != null)
                {
                    rate = nbpRes.Rates[0].Mid;
                }
            }
            catch (HttpRequestException)
            {
                try
                {
                    var nbpRes = await _httpClient.GetFromJsonAsync<NbpResponse>(linkB);
                    if (nbpRes != null)
                    {
                        rate = nbpRes.Rates[0].Mid;
                    }
                }
                catch (HttpRequestException)
                {
                    throw new BadRequestException("Could not get NBP Rates for " + currency);
                }
            }
            res = Math.Round(totalPln / rate, 2);
        }

        return new GetRevenueDetailsDto()
        {
            TotalAmount = res,
            Currency = cleanCurrency,
        };
    }
}