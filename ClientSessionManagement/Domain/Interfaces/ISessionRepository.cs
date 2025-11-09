using Domain.Entities;

namespace Domain.Interfaces;
public interface ISessionRepository
{
    Task AddAsync(Session session, CancellationToken cancellationToken);
    Task<Session?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Session?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken);
    void Update(Session session);
    void Remove(Session session);
    Task<Session?> ClientSessionAsync(Guid clientId, CancellationToken cancellationToken);
}
