using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Logic.Interfaces
{
    public interface ITaskRepository
    {
        void Add(Task task);
        void Update(Task task);
        void Delete(int id);

        Task? GetById(int id);
        List<Task> GetAll();
        List<Task> GetByStudentId(int studentId);
    }

}