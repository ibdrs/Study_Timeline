using Study_Timeline.Logic.Interfaces;

namespace Study_Timeline.Logic.Services
{
	public class TaskService
	{
		private readonly ITaskRepository _repo;

		public TaskService(ITaskRepository repo)
		{
			_repo = repo;
		}
	}
}
