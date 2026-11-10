using ChannelApp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ChannelApp.Controllers;

[ApiController]
[Route("api/mock")]
public class MockVendorController : ControllerBase
{
    [HttpGet("value")]
    public IActionResult GetValue()
    {
        // FR-04: Mock HTTP API returning 1
        return Ok(new MockValueResponse(1));
    }
}