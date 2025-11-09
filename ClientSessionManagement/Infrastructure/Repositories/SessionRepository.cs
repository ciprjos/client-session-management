using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class SessionRepository(ApplicationDbContext context) : ISessionRepository, IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(Session session, CancellationToken cancellationToken)
    {
        await _context.Sessions.AddAsync(session, cancellationToken);
    }
    public IQueryable<Session> GetAll()
    {
        return _context.Sessions.Include(x => x.Provider)
                                      .Include(x => x.Client)
                                      .Include(x => x.SessionType)
                                      .OrderBy(x => x.SessionDate)
                                      .AsQueryable();
    }
    public async Task<Session?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Sessions.Include(x => x.Client)
                                      .FirstOrDefaultAsync(s => s.Client.Name == name, cancellationToken);
    }
    public async Task<Session?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
    public void Update(Session session)
    {
        _context.Sessions.Update(session);
    }
    public void Remove(Session session)
    {
        _context.Sessions.Remove(session);
    }
    public async Task<Session?> ClientSessionAsync(Guid clientId, CancellationToken cancellationToken)
    {
        return await _context.Sessions.FirstOrDefaultAsync(s => s.ClientId == clientId, cancellationToken);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await _context.SaveChangesAsync(cancellationToken);
        return result;
    }
}
