using System.ComponentModel.DataAnnotations;
using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Shared.Models;

public class Status
{
    public int Id { get; set; }

    [Required]
    public ActivityStatus Name { get; set; }

    public string? Description { get; set; }
    public int Order { get; set; }
} 