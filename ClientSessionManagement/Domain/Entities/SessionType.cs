using Domain.Common;

namespace Domain.Entities;
public sealed class SessionType : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public ICollection<ProviderSessionType> Providers { get; set; } = [];
    public ICollection<Session> Sessions { get; set; } = [];
}