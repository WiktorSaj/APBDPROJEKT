using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBDPROJEKT.Entities;
[Table("IndividualClients")]
[Index(nameof(Pesel), IsUnique = true)]
public class IndividualClient : Client
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string Pesel  { get; set; } = string.Empty;
    [Required]
    public bool IsDeleted { get; set; } = false;
}