using APBDPROJEKT.DTOs.ClientDTOs;
using APBDPROJEKT.Entities;
using APBDPROJEKT.Exceptions;
using APBDPROJEKT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBDPROJEKT.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    
    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost("individual")]
    public async Task<IActionResult> AddIndividualClientAsync([FromBody]CreateIndividualClientDto dto)
    {
        try
        {
            await _clientService.CreateIndividualClientAsync(dto);
            return Created();
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPost("company")]
    public async Task<IActionResult> AddCompanyClientAsync([FromBody]CreateCompanyClientDto dto)
    {
        try
        {
            await _clientService.CreateCompanyClientAsync(dto);
            return Created();
        }
        catch (ConflictException e)
        {
            return  Conflict(e.Message);
        }
    }
    [HttpPut("individual/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateIndividualClientAsync([FromRoute]int id, [FromBody]UpdateIndividualClientDto dto)
    {
        try
        {
            await _clientService.UpdateIndividualClientAsync(id, dto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPut("company/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCompanyClientAsync([FromRoute]int id, [FromBody]UpdateCompanyClientDto dto)
    {
        try
        {
            await _clientService.UpdateCompanyClientAsync(id, dto);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteClientAsync(int id)
    {
        try
        {
            await _clientService.DeleteClientAsync(id);
            return NoContent();
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
}