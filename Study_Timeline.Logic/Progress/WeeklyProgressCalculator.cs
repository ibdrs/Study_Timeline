using Study_Timeline.Logic.Interfaces;
using Study_Timeline.Logic.Timeline;

namespace Study_Timeline.Logic.Progress
{
    public class WeeklyProgressCalculator : IProgressCalculator
    {
        public int CalculateWeekProgressPercent(TimelineWeekGroup week)
        {
            if (week.Tasks.Count == 0) return 0;

            var completed = week.Tasks.Count(t => t.Task.IsCompleted);
            var total = week.Tasks.Count;

            // Integer percentage
            return (int)Math.Round((completed * 100.0) / total);
        }
    }
}
