using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBDPROJEKT.Entities;

[Table("CompanyClients")]
[Index(nameof(Krs), IsUnique = true)]
public class CompanyClient : Client
{
    [Required]
    public string CompanyName { get; set; } = string.Empty;
    [Required]
    [StringLength(10, MinimumLength = 10)]
    public string Krs { get; set; } = string.Empty;
}