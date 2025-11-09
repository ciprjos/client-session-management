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

        return Ok(result);
    }

    [HttpGet("client-sessions")]
    public async Task<IActionResult> ClientSessions(CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetAllAsync(cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpGet("client-session")]
    public async Task<IActionResult> ClientSession([FromQuery] FilterSession filter, CancellationToken cancellationToken)
    {
        var result = await _sessionService.GetSessionsByClientNameAsync(filter, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpPut("update-session/{Id:guid}")]
    public async Task<IActionResult> UpdateSession([FromRoute] Guid Id, [FromBody] UpdateSessionDto updateSession, CancellationToken cancellationToken)
    {
        var result = await _sessionService.UpdateAsync(Id, updateSession, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpDelete("delete-session/{Id:guid}")]
    public async Task<IActionResult> DeleteSession([FromRoute] Guid Id, CancellationToken cancellationToken)
    {
        var result = await _sessionService.DeleteAsync(Id, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
