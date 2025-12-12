using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Interfaces;

namespace Study_Timeline.Logic.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _repo;

        public StudentService(IStudentRepository repo)
        {
            _repo = repo;
        }

        public Student? GetStudentByUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.");

            return _repo.GetByUser(username);
        }

        public void AddStudent(Student student)
        {
            _repo.Add(student);
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
