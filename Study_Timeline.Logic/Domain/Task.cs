namespace Study_Timeline.Logic.Domain
{
    public class Task
    {
        // Hydration constructor (repository)
        public Task(
            int id,
            int studentId,
            string title,
            string description,
            DateTime? startTime,
            DateTime? endTime,
            DateTime? deadline,
            int progressPercentage,
            bool isCompleted,
            Category? category)
        {
            Id = id;
            StudentId = studentId;
            Title = title;
            Description = description;

            ApplyTimeConstraints(startTime, endTime, deadline);

            ProgressPercentage = progressPercentage;
            IsCompleted = isCompleted;
            Category = category;
        }

        // Creation constructor (new task)
        public Task(int studentId, string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title cannot be empty.");

            StudentId = studentId;
            Title = title;
            Description = description;
            ProgressPercentage = 0;
            IsCompleted = false;
        }

        public int Id { get; private set; }
        public int StudentId { get; private set; }

        public string Title { get; private set; }
        public string Description { get; private set; }

        public DateTime? StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public DateTime? Deadline { get; private set; }

        public int ProgressPercentage { get; private set; }
        public bool IsCompleted { get; private set; }

        public Category? Category { get; set; }


        // Domain behaviour
        public void UpdateDetails(
            string title,
            string description,
            DateTime? startTime,
            DateTime? endTime,
            DateTime? deadline
            )
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.");

            Title = title;
            Description = description;

            ApplyTimeConstraints(startTime, endTime, deadline);
        }

        public void SetSchedule(DateTime start, DateTime end)
        {
            ApplyTimeConstraints(start, end, null);
        }

        public void SetDeadline(DateTime deadline)
        {
            ApplyTimeConstraints(null, null, deadline);
        }

        // A Task must have exactly ONE of:
        // - a schedule (StartTime + EndTime)
        // - OR a deadline
        // It may never have both, and never have neither.
        private void ApplyTimeConstraints(
            DateTime? start,
            DateTime? end,
            DateTime? deadline)
        {
            if (!deadline.HasValue && !(start.HasValue && end.HasValue))
                throw new InvalidOperationException("Task must have either a schedule or a deadline.");

            if (deadline.HasValue && (start.HasValue || end.HasValue))
                throw new InvalidOperationException("Task cannot have both a schedule and a deadline.");

            if (start.HasValue && end.HasValue && start > end)
                throw new InvalidOperationException("Start time cannot be after end time.");

            StartTime = start;
            EndTime = end;
            Deadline = deadline;
        }

        public void MarkCompleted()
        {
            ProgressPercentage = 100;
            IsCompleted = true;
        }

        public void UpdateProgress(int percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentOutOfRangeException();

            ProgressPercentage = percent;

            if (percent == 100)
                MarkCompleted();
        }
    }
}