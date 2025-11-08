using Domain.Common;

namespace Domain.Entities;
public sealed class Provider : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public ICollection<Session> Sessions { get; set; } = [];
    public ICollection<ProviderSessionType> ProviderSessionTypes { get; set; } = [];
}