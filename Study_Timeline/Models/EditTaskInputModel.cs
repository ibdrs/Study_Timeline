namespace Study_Timeline.Models
{
    public class EditTaskInputModel
    {
        // backing fields for default time values
        private DateTime _startTime = DateTime.Now;
        private DateTime _endTime = DateTime.Now.AddHours(1);

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime
        {
            get => new DateTime(_startTime.Year, _startTime.Month, _startTime.Day,
                                _startTime.Hour, _startTime.Minute, 0);

            set => _startTime = value;
        }

        public DateTime EndTime
        {
            get => new DateTime(_endTime.Year, _endTime.Month, _endTime.Day,
                                _endTime.Hour, _endTime.Minute, 0);

            set => _endTime = value;
        }
        public int ProgressPercentage { get; set; }
    }
}
