using Application.DTOs;
using Domain.Common;

namespace Application.Interfaces;
public interface ISessionService
{
    Task<Result<bool>> AddAsync(AddSessionDto addSessionDto, CancellationToken cancellationToken);
    Task<Result<List<GetSessionsDto>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<List<GetSessionsDto>>> GetSessionsByClientNameAsync(FilterSession filter, CancellationToken cancellationToken);
    Task<Result<bool>> UpdateAsync(Guid sessionId, UpdateSessionDto updateSessionDto, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteAsync(Guid sessionId, CancellationToken cancellationToken);
}
