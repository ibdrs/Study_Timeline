
namespace Study_Timeline.Logic.Domain
{
	public class Student
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }

        public List<Task> Tasks { get; set; } = new();

        // parameterloze constructor voor object initializers
        public Student() { }

        public Student(int id)
        {
            Id = id;
        }
        public Student(string name, string password) 
		{
			Name = name;
			Password = password;
		}
        public Student(int id, string name, string password)
        {
			Id = id;
            Name = name;
            Password = password;
        }

        // domain methods
        public void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            Tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            Tasks.Remove(task);
        }
    }
}
