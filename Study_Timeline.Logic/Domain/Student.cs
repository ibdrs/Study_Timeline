
namespace Study_Timeline.Logic.Domain
{
	public class Student
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }

		public List<Category>? Categories { get; set; } = new();
		public List<Task>? Tasks { get; set; } = new();

        public Student() { }

        public Student(int id, string name, string password) 
		{
			Id = id;
			Name = name;
			Password = password;
		}
        public Student(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
