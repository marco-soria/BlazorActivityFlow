using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Shared.DTOs.Activities;

public class ActivityDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int Priority { get; set; }
    
    public int StatusId { get; set; }
    public ActivityStatus Status { get; set; }
    
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    
    public string CreatedByUserId { get; set; } = string.Empty;
    public string CreatedByUserName { get; set; } = string.Empty;
    
    public string? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
} 