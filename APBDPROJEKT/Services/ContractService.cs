using APBDPROJEKT.Data;
using APBDPROJEKT.DTOs.ContractDTOs;
using APBDPROJEKT.Entities;
using APBDPROJEKT.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBDPROJEKT.Services;

public class ContractService : IContractService
{
    private readonly RevenueDbContext _dbContext;

    public ContractService(RevenueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateContractAsync(CreateContractDto dto)
    {
        if (dto.PaymentDeadlineDays < 3 || dto.PaymentDeadlineDays > 30)
        {
            throw new BadRequestException("Payment period must be between 3 and 30 days");
        }

        if (dto.BonusSupportYears < 0 || dto.BonusSupportYears > 3)
        {
            throw new BadRequestException("Bonus support years must be between 0 and 3");
        }
        
        var client = await _dbContext.Clients.Include(c=>c.Contracts).FirstOrDefaultAsync(c => c.Id == dto.ClientId);
        if (client == null || client is IndividualClient individualClient && individualClient.IsDeleted)
        {
            throw new NotFoundException("Client with given id was not found");
        }
        
        var software = await _dbContext.Software.Include(s=>s.Discounts).FirstOrDefaultAsync(s => s.Id == dto.SoftwareId);
        if (software == null)
        {
            throw new NotFoundException("Software with given id was not found");
        }

        var hasActiveContract = client.Contracts.Any(c =>
            c.SoftwareId == dto.SoftwareId &&
            ((!c.IsSigned && c.EndDate >= DateTime.Now) || (c.IsSigned && c.StartDate.AddYears(1 + c.BonusSupportYears) >= DateTime.Now))
        );

        if (hasActiveContract)
        {
            throw new ConflictException("This client already has active contract or an offer");
        }
        
        DateTime startDate = DateTime.Now;
        DateTime endDate = startDate.AddDays(dto.PaymentDeadlineDays);
        
        
        decimal highestDiscount = 0.0m;
        var discounts = software.Discounts.Where(d =>
            startDate >= d.StartDate && startDate <= d.EndDate
        ).ToList();

        if (discounts.Any())
        {
            highestDiscount = discounts.Max(d => d.Value);
            
        }

        var isReturning = client.Contracts.Any(c => c.IsSigned);
        
        decimal priceWithDiscount = software.BasePricePerYear * (1 -  highestDiscount/100);
        decimal priceWithDiscountAndBonusSupportPrice = priceWithDiscount + dto.BonusSupportYears * 1000m;
        decimal finalPrice = isReturning ? priceWithDiscountAndBonusSupportPrice * 0.95m :  priceWithDiscountAndBonusSupportPrice;
        var contract = new Contract
        {
            SoftwareVersion = software.Version,
            StartDate = startDate,
            EndDate = endDate,
            Price = finalPrice,
            BonusSupportYears = dto.BonusSupportYears,
            IsSigned = false,
            ClientId = dto.ClientId,
            SoftwareId = dto.SoftwareId,
        };
        
        await _dbContext.Contracts.AddAsync(contract);
        await _dbContext.SaveChangesAsync();
    }
}