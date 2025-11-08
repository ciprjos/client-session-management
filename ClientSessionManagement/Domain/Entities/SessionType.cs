using Domain.Common;

namespace Domain.Entities;
public sealed class SessionType : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public ICollection<ProviderSessionType> ProviderSessionTypes { get; set; } = [];
    public ICollection<Session> Sessions { get; set; } = [];
}