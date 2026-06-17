using APBDPROJEKT.DTOs.AuthenticationDTOs;
using APBDPROJEKT.Exceptions;
using APBDPROJEKT.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBDPROJEKT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
    {
        try
        {
            var tokens = await _authenticationService.LoginAsync(dto);
            return Ok(tokens);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshDto dto)
    {
        try
        {
            var tokens = await _authenticationService.RefreshTokenAsync(dto);
            return Ok(tokens);
        }catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    
}