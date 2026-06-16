using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPROJEKT.Entities;
[Table("Payments")]
public class Payment
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public int ContractId { get; set; }
    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; }
}