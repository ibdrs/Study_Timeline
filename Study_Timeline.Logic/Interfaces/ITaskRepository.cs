using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Logic.Interfaces
{
    public interface ITaskRepository
    {
		Task<List<Task>> GetAllAsync();
		Task<Task?> GetByIdAsync(int id);
		Task AddAsync(Task task);
		Task UpdateAsync(Task task);
		Task DeleteAsync(int id);
	}
}
