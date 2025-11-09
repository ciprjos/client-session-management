using Microsoft.EntityFrameworkCore;
using Bogus;
using Domain.Entities;

namespace Infrastructure.Data;
public sealed class DatabaseInitializer
{
    private readonly ApplicationDbContext _context;
    public DatabaseInitializer(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Seed()
    {
        var hasClients = await _context.Clients.AnyAsync();

        if (!hasClients)
        {
            var faker = new Faker<Client>()
                            .RuleFor(c => c.Id, f => Guid.NewGuid())
                            .RuleFor(c => c.Name, f => f.Person.FullName)
                            .RuleFor(c => c.Email, f => f.Person.Email)
                            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("###-###-####"));

            var clients = faker.Generate(50);

            await _context.Clients.AddRangeAsync(clients);

            await _context.SaveChangesAsync();
        }

        var hasSessionTypes = await _context.SessionTypes.AnyAsync();

        if (!hasSessionTypes)
        {
            var clientSessionTypes = new List<SessionType>
            {
                new() { Id = Guid.NewGuid(), Name = "Consultation" },
                new() { Id = Guid.NewGuid(), Name = "Therapy"},
                new() { Id = Guid.NewGuid(), Name = "Follow-up"},
                new() { Id = Guid.NewGuid(), Name = "Assessment"},
                new() { Id = Guid.NewGuid(), Name = "Workshop" }
            };

            await _context.SessionTypes.AddRangeAsync(clientSessionTypes);

            await _context.SaveChangesAsync();
        }

        var hasProviders = await _context.Providers.AnyAsync();

        if (!hasProviders) 
        {
            var providerFaker = new Faker<Provider>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Person.FullName);

            var providers = providerFaker.Generate(5);

            await _context.Providers.AddRangeAsync(providers);
        }

        var hasProviderSessionTypes = await _context.ProviderSessionTypes.AnyAsync();

        if (!hasProviderSessionTypes) 
        {
            var providers = await _context.Providers.ToListAsync();
            var sessionTypes = await _context.SessionTypes.ToListAsync();

            var providerSessionTypes = new List<ProviderSessionType>();

            foreach (var provider in providers)
            {
                var assignedSessionTypes = sessionTypes.OrderBy(_ => Guid.NewGuid()).ToList();
            
                foreach (var sessionType in assignedSessionTypes)
                {
                    providerSessionTypes.Add(new ProviderSessionType
                    {
                        ProviderId = provider.Id,
                        SessionTypeId = sessionType.Id
                    });
                }
            }

            await _context.ProviderSessionTypes.AddRangeAsync(providerSessionTypes);

            await _context.SaveChangesAsync();
        }
    }
}
