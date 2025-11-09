using Application.DTOs;
using Domain.Common;

namespace Application.Interfaces;
public interface ISessionService
{
    Task<Result<bool>> AddAsync(AddSessionDto addSessionDto, CancellationToken cancellationToken);
    Task<Result<List<GetSessionsDto>>> GetAllAsync(CancellationToken cancellationToken);
}
