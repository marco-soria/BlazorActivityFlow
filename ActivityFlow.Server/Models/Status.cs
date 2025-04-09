using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Server.Models;

public class Status
{
    [Key]
    public int Id { get; set; }

    [Required]
    public ActivityStatus Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    public int DisplayOrder { get; set; }

    // Relaciones
    public ICollection<Activity>? Activities { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
} 