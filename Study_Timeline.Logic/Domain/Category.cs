

namespace Study_Timeline.Logic.Domain
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int StudentId { get; set; } // FK
		public Student Student { get; set; }


		public List<Task> Tasks { get; set; } = new()
	}
}
