

namespace Study_Timeline.Logic.Domain
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsCompleted { get; set; }
        public int ProgressPercentage { get; set; }

        public Task() { }

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
