using DomainTask = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Logic.Interfaces
{
	public interface ITaskRepository
	{
		Task<List<DomainTask>> GetAllAsync();
		Task<DomainTask?> GetByIdAsync(int id);
		Task AddAsync(DomainTask task);
		Task UpdateAsync(DomainTask task);
		Task DeleteAsync(int id);
	}
}