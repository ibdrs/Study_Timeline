

namespace Study_Timeline.Logic.Domain
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
		public DateTime? Deadline { get; set; }
        public int ProgressPercentage { get; set; }
		public bool IsCompleted { get; set; }

		public int StudentId { get; set; } // FK
		public Student Student { get; set; }

		public int? CategoryId { get; set; } // FK (Nullable)
		public Category? Category { get; set; }

		// Domain methods
		public void MarkCompleted()
		{
			ProgressPercentage = 100;
			IsCompleted = true;
		}
		public bool HasValidSchedule()
		{
			return StartTime <= EndTime;
		}
        public void UpdateProgress(int percent)
        {
			ProgressPercentage = percent;

			if (percent >= 100)
                MarkCompleted();
        }
	}
}
