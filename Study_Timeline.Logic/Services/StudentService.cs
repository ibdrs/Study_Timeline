using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Interfaces;

namespace Study_Timeline.Logic.Services
{
    public class StudentService :
        IStudentAuthenticationService,
        IStudentRegistrationService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ITaskRepository _taskRepo;

        public StudentService(IStudentRepository studentRepo, ITaskRepository taskRepo)
        {
            _studentRepo = studentRepo;
            _taskRepo = taskRepo; 
        }

        private Student? GetStudentByUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.");

            return _studentRepo.GetByUser(username);
        }

        public void AddTaskForStudent(
            int studentId,
            string title,
            string description,
            DateTime? start,
            DateTime? end,
            DateTime? deadline)
        {
            var student = _studentRepo.GetById(studentId)
                ?? throw new InvalidOperationException("Student not found");

            if (deadline == null && (start == null || end == null))
                throw new InvalidOperationException(
                    "Task must have either a deadline or a start and end time."
                );

            var task = student.AddTask(title, description);

            if (deadline.HasValue)
            {
                task.SetDeadline(TrimSeconds(deadline.Value));
            }
            else
            {
                task.SetSchedule(
                    TrimSeconds(start!.Value),
                    TrimSeconds(end!.Value)
                );
            }

            _taskRepo.Add(task, student.Id);
        }

        private static DateTime TrimSeconds(DateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        
        // authentication logic
        public void RegisterStudent(Student student)
        {
            _studentRepo.Add(student);
        }

        public Student? ValidateStudent(string username, string password)
        {
            var student = GetStudentByUser(username);

            if (student == null)
                return null;

            if (string.IsNullOrWhiteSpace(password))
                return null;

            if (password != student.Password)
                return null;

            return student;
        }
    }
}
