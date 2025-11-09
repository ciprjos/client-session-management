using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class ProviderRepository(ApplicationDbContext context) : IProviderRepository
{
    private readonly ApplicationDbContext _context = context;
    public async Task<Provider?> GetByIdAsync(Guid providerId, CancellationToken cancellationToken)
    {
         return await _context.Providers
                                .Include(x => x.ProviderSessionTypes)
                                .FirstOrDefaultAsync(p => p.Id == providerId, cancellationToken);
    }
}
