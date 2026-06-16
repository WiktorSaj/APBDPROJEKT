namespace APBDPROJEKT.DTOs.PaymentDTOs;

public class CreatePaymentDto
{
    public int ContractId { get; set; }
    public decimal Amount { get; set; }
    public int ClientId { get; set; }
}