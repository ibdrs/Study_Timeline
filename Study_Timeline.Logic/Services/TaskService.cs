using Study_Timeline.Logic.Interfaces;
using Task = Study_Timeline.Logic.Domain.Task;


namespace Study_Timeline.Logic.Services
{
	public class TaskService
	{
		private readonly ITaskRepository _repo;

		public TaskService(ITaskRepository repo)
		{
			_repo = repo;
		}

		public List<Task> GetAllTasks()
		{
			return _repo.GetAll();
		}


	}
}
