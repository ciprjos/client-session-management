namespace Application.DTOs;
public sealed class GetSessionsDto
{
    public Guid SessionId { get; set; }
    public string ClientName { get; set; } = default!;
    public string ProviderName { get; set; } = default!;
    public string SessionType { get; set; } = default!; 
    public DateTime SessionDate { get; set; }
    public string? Notes { get; set; }
}
