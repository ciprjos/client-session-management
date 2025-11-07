using Domain.Common;

namespace Domain.Entities;
public sealed class SessionType : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public ICollection<Provider> Providers { get; set; } = [];
    public ICollection<Session> Sessions { get; set; } = [];
}