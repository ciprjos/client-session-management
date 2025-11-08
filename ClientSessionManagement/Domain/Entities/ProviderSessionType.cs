namespace Domain.Entities;
public sealed class ProviderSessionType
{
    public Guid ProviderId { get; set; }
    public Provider Provider { get; set; } = default!;
    public Guid SessionTypeId { get; set; }
    public SessionType SessionType { get; set; } = default!;
}
