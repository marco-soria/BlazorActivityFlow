using System.ComponentModel.DataAnnotations;

namespace ActivityFlow.Server.Models;

public class Comment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public int ActivityId { get; set; }

    // Relaciones
    public ApplicationUser? User { get; set; }
    public Activity? Activity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
} 