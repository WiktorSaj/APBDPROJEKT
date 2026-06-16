using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPROJEKT.Entities;
[Table("Contracts")]
public class Contract
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string SoftwareVersion { get; set; } = string.Empty;
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [Required]
    [Range(0, 3)]
    public int BonusSupportYears { get; set; }
    [Required]
    public bool IsSigned { get; set; } =  false;
    [Required]
    public int ClientId { get; set; }
    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }
    [Required]
    public int SoftwareId { get; set; }
    [ForeignKey(nameof(SoftwareId))]
    public Software Software { get; set; }
    
    public ICollection<Payment> Payments { get; set; } = [];
    
}