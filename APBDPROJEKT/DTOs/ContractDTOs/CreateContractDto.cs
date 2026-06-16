namespace APBDPROJEKT.DTOs.ContractDTOs;

public class CreateContractDto
{
    public int ClientId {get; set;}
    public int SoftwareId {get; set;}
    public int BonusSupportYears {get; set;}
    public int PaymentDeadlineDays {get; set;}
}