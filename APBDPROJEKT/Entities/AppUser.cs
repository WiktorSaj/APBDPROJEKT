using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APBDPROJEKT.Entities;

[Table("AppUsers")]
[Index(nameof(Login), IsUnique = true)]
public class AppUser
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Login { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Role { get; set; } = string.Empty;
    
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpires { get; set; }
}