using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Exceptions;
using Study_Timeline.Logic.Interfaces;
using Study_Timeline.Logic.Interfaces.Data;

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
                throw new ValidationException("Username cannot be empty.", field: "Username");

            return _studentRepo.GetByUser(username);
        }

        public void RegisterStudent(Student student)
        {
            var existing = _studentRepo.GetByUser(student.Name);
            if (existing != null)
                throw new ValidationException("Username already exists.", field: "Name");

            _studentRepo.Add(student);
        }

        public Student? ValidateStudent(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            if (string.IsNullOrWhiteSpace(password))
                return null;

            var student = GetStudentByUser(username);
            if (student == null)
                return null;

            if (password != student.Password)
                return null;

            return student;
        }
    }
}
