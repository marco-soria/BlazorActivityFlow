using ActivityFlow.Shared.Enums;

namespace ActivityFlow.Server.Models;

public class Status
{
    public int Id { get; set; }
    public ActivityStatus Name { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
} 