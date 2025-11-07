using Domain.Common;

namespace Domain.Entities;
public sealed class Session : BaseEntity<Guid>
{
    public Guid ClientId { get; set; }
    public Guid SessionTypeId { get; set; }
    public Guid ProviderId { get; set; }
    public DateTime SessionDate { get; set; }
    public string? Notes { get; set; }
    public Provider Provider { get; set; } = default!;
    public Client Client { get; set; } = default!;
    public SessionType SessionType { get; set; } = default!;
}
