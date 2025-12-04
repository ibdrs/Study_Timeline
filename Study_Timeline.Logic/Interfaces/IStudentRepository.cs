using Study_Timeline.Logic.Domain;

namespace Study_Timeline.Logic.Interfaces
{
    public interface IStudentRepository
    {
        Student? GetByEmail(string email);
        void Add(Student student);
        void Update(Student student);
    }
}
