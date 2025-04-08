namespace ActivityFlow.Shared.DTOs.Activities;

public class CreateActivityDTO
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int Priority { get; set; }
    public int StatusId { get; set; }
    public int? CategoryId { get; set; }
    public string? AssignedToUserId { get; set; }
} 