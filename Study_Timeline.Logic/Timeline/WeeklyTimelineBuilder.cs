using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Interfaces;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Logic.Timeline
{
    public class WeeklyTimelineBuilder : ITimelineBuilder
    {
        public IReadOnlyList<TimelineWeekGroup> BuildWeekly(IReadOnlyList<Task> tasks)
        {
            if (tasks == null)
            {
                tasks = [];
            }

            // Convert tasks to timeline entries
            var entries = tasks
                .Select(task => new TimelineTaskEntry(task)) // every task gets an entry object
                .OrderBy(entry => entry.Anchor) // sorting entries chronologically by date
                .ToList();

            if (entries.Count == 0)
                return [];

            var groups = entries
                .GroupBy(entry => GetWeekStart(DateOnly.FromDateTime(entry.Anchor)))
                .OrderBy(g => g.Key)
                .Select(g =>
                {
                    var ws = g.Key;
                    var we = ws.AddDays(6);
                    return new TimelineWeekGroup(ws, we, g.ToList());
                })
                .ToList();

            return groups;
        }

        private static (DateOnly weekStart, DateOnly weekEnd) GetWeekBounds(DateOnly date)
        {
            var ws = GetWeekStart(date);
            return (ws, ws.AddDays(6));
        }

        private static DateOnly GetWeekStart(DateOnly date)
        {
            int dow = (int)date.DayOfWeek; // 0 = sunday, 6 = saturday
            int daysSinceMonday;

            switch (dow)
            {
                case 1:
                    daysSinceMonday = 0;
                    break;

                case 2:
                    daysSinceMonday = 1;
                    break;

                case 3: 
                    daysSinceMonday = 2;
                    break;

                case 4: 
                    daysSinceMonday = 3;
                    break;

                case 5:
                    daysSinceMonday = 4;
                    break;

                case 6:
                    daysSinceMonday = 5;
                    break;

                case 0:
                    daysSinceMonday = 6;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return date.AddDays(-daysSinceMonday);
        }
    }
}
