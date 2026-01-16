namespace Study_Timeline.Models.Timeline
{
    public sealed class TimelineWeekVM
    {
        public DateOnly WeekStart { get; set; }
        public DateOnly WeekEnd { get; set; }

        public int IsoWeekNumber { get; set; }
        public int IsoWeekYear { get; set; }

        public int ProgressPercent { get; set; }
        public List<TimelineTaskItemVM> Tasks { get; set; } = new();
    }
}
