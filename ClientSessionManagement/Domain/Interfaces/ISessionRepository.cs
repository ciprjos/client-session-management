using Domain.Entities;
using System.Linq;

namespace Domain.Interfaces;
public interface ISessionRepository
{
    Task AddAsync(Session session, CancellationToken cancellationToken);
    Task<Session?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Session?> GetByNameAsync(string name, CancellationToken cancellationToken);
    IQueryable<Session> GetAll();
    void Update(Session session);
    void Remove(Session session);
    Task<Session?> ClientSessionAsync(Guid clientId, CancellationToken cancellationToken);
}
