using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Study_Timeline.Logic.Interfaces;
using Task = Study_Timeline.Logic.Domain.Task;


namespace Study_Timeline.Data.Repositories
{
	public class TaskRepository : ITaskRepository
	{
		private readonly DbConnectionFactory _factory;
		public TaskRepository(DbConnectionFactory factory)
		{
			_factory = factory;
		}

		public Task AddAsync(Task task)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<Task>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Task?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Task task)
		{
			throw new NotImplementedException();
		}
	}
}
