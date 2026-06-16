
using APBDPROJEKT.DTOs.ContractDTOs;
using APBDPROJEKT.Exceptions;
using APBDPROJEKT.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBDPROJEKT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;
    
    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateContractAsync([FromBody] CreateContractDto dto)
    {
        try
        {
            await _contractService.CreateContractAsync(dto);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
}