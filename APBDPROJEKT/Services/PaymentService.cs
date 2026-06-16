using APBDPROJEKT.Data;
using APBDPROJEKT.DTOs.PaymentDTOs;
using APBDPROJEKT.Entities;
using APBDPROJEKT.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBDPROJEKT.Services;

public class PaymentService : IPaymentService
{
    
    private readonly RevenueDbContext _dbContext;
    
    public PaymentService(RevenueDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreatePaymentAsync(CreatePaymentDto dto)
    {
        var contract = await _dbContext.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.Id == dto.ContractId);


        if (contract == null)
        {
            throw new NotFoundException("Contract with given id was not found");
        }

        if (contract.ClientId != dto.ClientId)
        {
            throw new BadRequestException("This contract does not belong to given client");
        }

        if (contract.IsSigned)
        {
            throw new ConflictException("This contract is already signed");
        }

        if (DateTime.Now > contract.EndDate)
        {
            throw new BadRequestException("The payment deadline for this contract has expired");
        }
        
        decimal alreadyPaid = contract.Payments.Sum(p => p.Amount);
        decimal remaining = contract.Price - alreadyPaid;

        if (dto.Amount > remaining)
        {
            throw new BadRequestException("Payment amount is larger than it should be. Remaining amount to pay is " + remaining + " PLN");
        }

        var payment = new Payment
        {
            Amount = dto.Amount,
            Date = DateTime.Now,
            ContractId = dto.ContractId,
        };
        await _dbContext.Payments.AddAsync(payment);

        if (dto.Amount == remaining)
        {
            contract.IsSigned = true;
        }
        
        await _dbContext.SaveChangesAsync();
    }
}