using System.ComponentModel.DataAnnotations;

namespace ActivityFlow.Shared.Models;

public class Activity
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int Priority { get; set; } = 0;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int StatusId { get; set; }
    public Status? Status { get; set; }

    public string? UserId { get; set; }
} 