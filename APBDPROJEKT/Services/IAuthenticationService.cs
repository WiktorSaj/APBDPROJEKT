using APBDPROJEKT.DTOs.AuthenticationDTOs;

namespace APBDPROJEKT.Services;

public interface IAuthenticationService
{
    Task<GetTokensDto?> LoginAsync(LoginDto loginDto);
    Task<GetTokensDto?> RefreshTokenAsync(RefreshDto refreshDto);
}