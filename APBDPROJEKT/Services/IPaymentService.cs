using APBDPROJEKT.DTOs.PaymentDTOs;

namespace APBDPROJEKT.Services;

public interface IPaymentService
{
    Task CreatePaymentAsync(CreatePaymentDto dto);
}