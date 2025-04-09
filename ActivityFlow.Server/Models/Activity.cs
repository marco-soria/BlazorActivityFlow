using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Server.Models;

public class Activity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int StatusId { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal? Progress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public int Priority { get; set; } = 0;

    public DateTime? DueDate { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? AssignedToId { get; set; }

    // Relaciones
    [ForeignKey(nameof(UserId))]
    public ApplicationUser? User { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }

    [ForeignKey(nameof(StatusId))]
    public Status? Status { get; set; }

    [ForeignKey(nameof(AssignedToId))]
    public ApplicationUser? AssignedTo { get; set; }
} 