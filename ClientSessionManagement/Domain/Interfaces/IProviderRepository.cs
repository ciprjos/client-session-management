using Domain.Entities;

namespace Domain.Interfaces;
public interface IProviderRepository
{
    Task<Provider?> GetByIdAsync(Guid providerId, CancellationToken cancellationToken);
}
