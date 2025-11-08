using Domain.Entities;

namespace Domain.Interfaces;
public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
