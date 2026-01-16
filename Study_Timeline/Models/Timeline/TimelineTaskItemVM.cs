namespace Study_Timeline.Models.Timeline
{
    public sealed class TimelineTaskItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime Anchor { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? Deadline { get; set; }

        public bool IsCompleted { get; set; }
        public int ProgressPercentage { get; set; }

        public string? CategoryName { get; set; }
        public int DurationMinutes { get; set; }
    }
}
