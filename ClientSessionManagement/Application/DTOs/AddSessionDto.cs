namespace Application.DTOs;
public sealed record AddSessionDto(Guid ClientId, Guid SessionTypeId, Guid ProviderId, DateTime SessionDate, string? Notes);