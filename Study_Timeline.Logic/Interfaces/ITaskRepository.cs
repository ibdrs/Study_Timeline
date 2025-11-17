using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Logic.Interfaces
{
	public interface ITaskRepository
	{
		List<Task> GetAll();
		Task? GetById(int id);
		void Add(Task task);
		void Update(Task task);
		void Delete(int id);
	}
}