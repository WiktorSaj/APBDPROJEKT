using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPROJEKT.Entities;
[Table("Discounts")]
public class Discount
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    [Range(0, 100)]
    [Column(TypeName = "decimal(4,2)")]
    public decimal Value { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public int SoftwareId { get; set; }
    
    [ForeignKey(nameof(SoftwareId))]
    public Software Software { get; set; }
}