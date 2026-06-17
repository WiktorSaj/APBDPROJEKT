namespace APBDPROJEKT.DTOs.AuthenticationDTOs;

public class GetTokensDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}