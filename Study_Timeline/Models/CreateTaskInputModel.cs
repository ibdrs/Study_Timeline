public class CreateTaskInputModel
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;

    public bool IsDeadline { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? Deadline { get; set; }
}
