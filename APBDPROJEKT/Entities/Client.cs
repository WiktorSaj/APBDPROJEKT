using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDPROJEKT.Entities;

[Table("Clients")]
public abstract class Client
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Address { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    public ICollection<Contract> Contracts { get; set; } = [];
}