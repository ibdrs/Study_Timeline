namespace Study_Timeline.Logic.Domain
{
    public class Student
    {
        // Hydration constructor (repository)
        public Student(int id, string name, string password)
        {
            Id = id;
            SetName(name);
            SetPassword(password);
        }

        // Creation constructor (new student)
        public Student(string name, string password)
        {
            SetName(name);
            SetPassword(password);
        }

        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;

        public List<Task> Tasks { get; set; } = new();

        // Domain behaviour

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            Name = name;
        }

        private void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            Password = password;
        }

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
