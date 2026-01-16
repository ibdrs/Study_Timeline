using Study_Timeline.Logic.Domain;

namespace Study_Timeline.Logic.Timeline
{
    /// <summary>
    /// Domain-friendly weekly grouping used to render a timeline.
    /// </summary>
    public class TimelineWeekGroup
    {
        public DateOnly WeekStart { get; }
        public DateOnly WeekEnd { get; }
        public int IsoWeekNumber { get; }
        public int IsoWeekYear { get; }
        public IReadOnlyList<TimelineTaskEntry> Tasks { get; }

        public TimelineWeekGroup(DateOnly weekStart, DateOnly weekEnd, IReadOnlyList<TimelineTaskEntry> tasks)
        {
            WeekStart = weekStart;
            WeekEnd = weekEnd;
            Tasks = tasks;

            var dt = weekStart.ToDateTime(TimeOnly.MinValue);
            IsoWeekNumber = System.Globalization.ISOWeek.GetWeekOfYear(dt);
            IsoWeekYear = System.Globalization.ISOWeek.GetYear(dt);
        }
    }
}
