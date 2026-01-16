using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Interfaces;
using Study_Timeline.Logic.Interfaces.Data;
using Task = Study_Timeline.Logic.Domain.Task;

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
