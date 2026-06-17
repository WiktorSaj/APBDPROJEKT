using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using APBDPROJEKT.Data;
using APBDPROJEKT.DTOs.AuthenticationDTOs;
using APBDPROJEKT.Exceptions;
using APBDPROJEKT.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APBDPROJEKT.Services;

public class AuthenticationService : IAuthenticationService
{
    
    private readonly IConfiguration _configuration;
    private readonly RevenueDbContext _dbContext;

    public AuthenticationService(RevenueDbContext dbContext, IConfiguration configuration)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }
    
    public async Task<GetTokensDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u =>u.Login == loginDto.Username);
        if (user == null)
        {
            throw new NotFoundException("Invalid username provided");
        }
        bool valid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
        if (!valid)
        {
            throw new BadRequestException("Invalid password provided");
        }
        
        var tokens = GenerateTokens(user);
        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpires =  DateTime.UtcNow.AddDays(7);
        
        await _dbContext.SaveChangesAsync();
        return tokens;
        
    }

    public async Task<GetTokensDto?> RefreshTokenAsync(RefreshDto refreshDto)
    {
        var user = await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.RefreshToken == refreshDto.RefreshToken);
        if (user == null)
        {
            throw new NotFoundException("Invalid refresh token provided");
        }

        if (user.RefreshTokenExpires < DateTime.UtcNow)
        {
            throw new BadRequestException("Refresh token has expired");
        }
        
        var tokens = GenerateTokens(user);
        
        user.RefreshToken = tokens.RefreshToken;
        user.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);
        await _dbContext.SaveChangesAsync();
        return tokens;
    }


    private GetTokensDto GenerateTokens(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.Role, user.Role),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        var refreshToken = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64)
        );

        return new GetTokensDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}