using Domain.Common;

namespace Domain.Entities;
public sealed class Client : BaseEntity<Guid>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public ICollection<Session> Sessions { get; set; } = [];
}
