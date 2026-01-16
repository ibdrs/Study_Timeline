using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Timeline;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Logic.Interfaces
{
    /// <summary>
    /// Builds a weekly timeline representation for a set of tasks.
    /// Pure domain logic (no database / UI concerns).
    /// </summary>
    public interface ITimelineBuilder
    {
        IReadOnlyList<TimelineWeekGroup> BuildWeekly(IReadOnlyList<Task> tasks);
    }
}
