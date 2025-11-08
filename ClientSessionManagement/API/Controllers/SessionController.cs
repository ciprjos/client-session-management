using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;
    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("add-client-session")]
    public async Task<IActionResult> AddClientSession([FromBody] AddSessionDto addSession, CancellationToken cancellationToken)
    {
        var result = await _sessionService.AddAsync(addSession, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok("AddClientSession endpoint is working.");
    }
}
