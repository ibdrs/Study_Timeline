namespace Study_Timeline.Logic.Domain
{
    public class Task
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
		public DateTime? Deadline { get; set; }
        public int ProgressPercentage { get; set; }
		public bool IsCompleted { get; set; }

		public Student Student { get; set; }
		public Category? Category { get; set; }

		// Domain methods
		public void MarkCompleted()
		{
			ProgressPercentage = 100;
			IsCompleted = true;
		}
        public void UpdateProgress(int percent)
        {
			if (percent < 0 || percent > 100)
				throw new ArgumentOutOfRangeException("Percentage must be valid.");

			ProgressPercentage = percent;

			if (percent == 100 && !IsCompleted)
                MarkCompleted();
        }
		public void SetSchedule(DateTime start, DateTime end)
		{
			if (start > end)
				throw new InvalidOperationException("Start time cannot be after end time.");

			StartTime = start;
			EndTime = end;
		}
	}
}
