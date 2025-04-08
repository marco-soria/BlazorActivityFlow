using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Shared.DTOs.Status;

public class StatusDTO
{
    public int Id { get; set; }
    public ActivityStatus Name { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
} 