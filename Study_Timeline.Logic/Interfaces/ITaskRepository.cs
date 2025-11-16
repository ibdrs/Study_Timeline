using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Logic.Interfaces
{
	public interface ITaskRepository
	{
		Task<List<Task>> GetAllAsync();
		Task<Task?> GetByIdAsync(int id);
		System.Threading.Tasks.Task AddAsync(Task task);
		System.Threading.Tasks.Task UpdateAsync(Task task);
		System.Threading.Tasks.Task DeleteAsync(int id);
	}
}