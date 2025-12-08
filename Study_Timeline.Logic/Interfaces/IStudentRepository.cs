using Study_Timeline.Logic.Domain;

namespace Study_Timeline.Logic.Interfaces
{
    public interface IStudentRepository
    {
        Student? GetByUser(string username);
        void Add(Student student);
        void Update(Student student);
    }
}
