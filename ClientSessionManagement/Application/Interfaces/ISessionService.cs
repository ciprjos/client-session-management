using Application.DTOs;
using Domain.Common;

namespace Application.Interfaces;
public interface ISessionService
{
    Task<Result<bool>> AddAsync(AddSessionDto addSessionDto, CancellationToken cancellationToken);
}
