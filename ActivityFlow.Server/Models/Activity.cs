using Microsoft.AspNetCore.Identity;

namespace ActivityFlow.Server.Models;

public class Activity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int Priority { get; set; } = 0;

    public int StatusId { get; set; }
    public virtual Status Status { get; set; } = null!;

    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }

    public string UserId { get; set; } = string.Empty;
    public virtual IdentityUser User { get; set; } = null!;

    public string? AssignedToId { get; set; }
    public virtual IdentityUser? AssignedTo { get; set; }
} 