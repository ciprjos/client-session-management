using Domain.Common;

namespace Domain.Entities;
public sealed class Provider : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public Guid SessionTypeId { get; set; }
    public SessionType SessionType { get; set; } = default!;
    public ICollection<Session> Sessions { get; set; } = [];
}