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
                throw new Exception("Username cannot be empty.");

            return _repo.GetByUser(username);
        }

        public void AddStudent(Student s)
        {
            if (string.IsNullOrWhiteSpace(s.Name))
                throw new Exception("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(s.Password))
                throw new Exception("Password cannot be empty.");

            _repo.Add(s);
        }

        // Check if the student password is correct
        public Student? ValidateStudent(string username, string password)
        {
            // student object from our database
            var student = GetStudentByUser(username);

            if (student == null)
                return null;

            if (password == null)
                return null;

            if (password != student.Password)
                return null;

            // if our checks dont return null
            return student;
        }
    }
}
