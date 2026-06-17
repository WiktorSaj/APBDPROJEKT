using APBDPROJEKT.Exceptions;
using APBDPROJEKT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBDPROJEKT.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }


    [HttpGet("realized")]
    public async Task<IActionResult> GetRealizedRevenueAsync([FromQuery] int? softwareId,
        [FromQuery] string? currency = "PLN")
    {
        try
        {
            var res = await _revenueService.CalculateRealizedRevenueAsync(softwareId, currency);
            return Ok(res);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    
    
    
    [HttpGet("forecasted")]
    public async Task<IActionResult> GetForecastedRevenueAsync([FromQuery] int? softwareId,
        [FromQuery] string? currency = "PLN")
    {
        try
        {
            var res = await _revenueService.CalculateForecastedRevenueAsync(softwareId, currency);
            return Ok(res);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    
}