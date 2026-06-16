namespace APBDPROJEKT.DTOs.RevenueDTOs;

public class GetRevenueDetailsDto
{
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } =  "PLN";
}