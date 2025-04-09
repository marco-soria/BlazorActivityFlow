using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Shared.Models;

public class CreateActivityDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
}

public class UpdateActivityDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public string Status { get; set; } = "Pending"; // Ahora usamos el nombre del estado
}

public class ActivityResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public int Priority { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public string? AssignedTo { get; set; }
} 