using Study_Timeline.Logic.Domain;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Logic.Timeline
{
    /// <summary>
    /// A lightweight timeline item derived from a Task.
    /// </summary>
    public class TimelineTaskEntry
    {
        public Task Task { get; }
        /// <summary>
        /// When the item should be positioned on the timeline.
        /// For scheduled tasks: StartTime. For deadline tasks: Deadline.
        /// </summary>
        public DateTime Anchor { get; }

        public int DurationMinutes { get; }

        public TimelineTaskEntry(Task task)
        {
            Task = task ?? throw new ArgumentNullException(nameof(task));
            Anchor = task.StartTime ?? task.Deadline ?? throw new InvalidOperationException("Task has no schedule/deadline.");

            if (task.StartTime.HasValue && task.EndTime.HasValue)
                DurationMinutes = (int)Math.Max(1, (task.EndTime.Value - task.StartTime.Value).TotalMinutes);
            else
                DurationMinutes = 30; // default size for deadline-only tasks
        }
    }
}
