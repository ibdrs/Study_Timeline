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

        public Student? GetStudentByUser(string user)
        {
            if (string.IsNullOrWhiteSpace(user))
                throw new Exception("Username cannot be empty.");

            return _repo.GetByUser(user);
        }

        public void AddStudent(Student s)
        {
            if (string.IsNullOrWhiteSpace(s.Name))
                throw new Exception("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(s.Password))
                throw new Exception("Password cannot be empty.");

            _repo.Add(s);
        }
    }
}
