using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;
    public SessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Session session, CancellationToken cancellationToken)
    {
        await _context.Sessions.AddAsync(session, cancellationToken);
    }
    public async Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Sessions.ToListAsync(cancellationToken);
    }
    public async Task<Session?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Sessions.FirstOrDefaultAsync(s => s.Notes == name, cancellationToken);
    }
    public async Task<Session?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
    public void Update(Session session)
    {
        _context.Sessions.Update(session);
    }
}
