namespace Application.DTOs;
public sealed record UpdateSessionDto(Guid ClientId, 
                                      Guid SessionTypeId,
                                      Guid ProviderId, 
                                      DateTime SessionDate,
                                      string? Notes);