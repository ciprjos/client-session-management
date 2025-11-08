using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class ClientRepository(ApplicationDbContext context) : IClientRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Clients.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
