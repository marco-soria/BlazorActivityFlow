using System.ComponentModel.DataAnnotations;

namespace ActivityFlow.Shared.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#000000";
    public bool IsActive { get; set; }
}

public class CreateCategoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public string Color { get; set; } = "#000000";
}

public class UpdateCategoryDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public string? Color { get; set; }

    public bool? IsActive { get; set; }
} 