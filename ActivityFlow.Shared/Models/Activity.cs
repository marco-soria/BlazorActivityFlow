using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityFlow.Shared.Models;

public class Activity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedAt { get; set; }

    public int Priority { get; set; } = 0;

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

    [Required]
    public int StatusId { get; set; }

    [ForeignKey(nameof(StatusId))]
    public Status? Status { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public string? AssignedToId { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Progress { get; set; }

    public DateTime? UpdatedAt { get; set; }
} 