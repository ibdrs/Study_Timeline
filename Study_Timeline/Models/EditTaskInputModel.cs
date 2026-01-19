namespace Study_Timeline.Models
{
    public class EditTaskInputModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsDeadline { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? Deadline { get; set; }
        public int? SelectedCategoryId { get; set; }

        public string? NewCategoryName { get; set; }
        public string? NewCategoryDescription { get; set; }

        public int ProgressPercentage { get; set; }
    }
}
