using ChannelApp.DTOs;
using ChannelApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChannelApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculationsController : ControllerBase
{
    private readonly ICalculationService _service;

    public CalculationsController(ICalculationService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CalculationResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(415)]
    public async Task<IActionResult> Post([FromBody] CalculationRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var response = await _service.CreateRecordAsync(request);
        return CreatedAtAction(nameof(GetSum), new { id = response.Id }, response);
    }

    [HttpGet("{id}/sum")]
    [ProducesResponseType(typeof(CalculationSumResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    [ProducesResponseType(502)]
    [ProducesResponseType(504)]
    public async Task<IActionResult> GetSum(int id)
    {
        if (id <= 0) return BadRequest(new { title = "Invalid ID", detail = "ID must be a positive integer." });

        try
        {
            var result = await _service.CalculateSumAsync(id);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { title = "Not Found", detail = ex.Message });
        }
    }
}