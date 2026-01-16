using Study_Timeline.Logic.Timeline;

namespace Study_Timeline.Logic.Interfaces
{
    /// <summary>
    /// Calculates progress values for a set of timeline weeks.
    /// </summary>
    public interface IProgressCalculator
    {
        int CalculateWeekProgressPercent(TimelineWeekGroup week);
    }
}
