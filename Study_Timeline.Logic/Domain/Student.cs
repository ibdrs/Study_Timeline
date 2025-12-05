
namespace Study_Timeline.Logic.Domain
{
	public class Student
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }

		public List<Category>? Categories { get; set; } = new();
		public List<Task>? Tasks { get; set; } = new();

		// parameterloze constructor voor object initializers
		public Student() { }

        public Student(string name, string password) 
		{
			Name = name;
			Password = password;
		}

		public Student(int id)
		{
			Id = id;
		}
	}
}
