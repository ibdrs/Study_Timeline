namespace Study_Timeline.Logic.Domain
{
    public class Student
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }

        private readonly List<Task> _tasks = new();

        public IReadOnlyList<Task> Tasks {
            get { return _tasks; }
        }

        // Hydration constructor (repository)
        public Student(int id, string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            Id = id;
            Name = name;
            Password = password;
        }

        // Creation constructor (new student)
        public Student(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            Name = name;
            Password = password;
        }

        // Domain behaviour
        public Task AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _tasks.Add(task);
            return task;
        }

        public void RemoveTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _tasks.Remove(task);
        }
    }
}
