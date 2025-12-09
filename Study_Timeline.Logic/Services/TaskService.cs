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

		// Get all tasks
		public List<Task> GetAllTasks()
		{
			return _repo.GetAll();
		}

		// Get a task by Id
		public Task? GetTaskById(int id)
		{
			return _repo.GetById(id);
		}

		// Add a new task
		public void AddTask(Task task)
		{
			if (string.IsNullOrWhiteSpace(task.Title))
				throw new ArgumentException("Task title cannot be empty.");

			_repo.Add(task);
		}

		// Update an existing task
		public void UpdateTask(Task task)
		{
			var existing = _repo.GetById(task.Id);
			if (existing == null)
				throw new KeyNotFoundException($"Task with Id {task.Id} not found.");
			if (existing.IsCompleted)
				throw new InvalidOperationException("Cannot edit completed tasks.");

			_repo.Update(task);
		}

		// Delete a task
		public void DeleteTask(int id)
		{
			var existing = _repo.GetById(id);
			if (existing == null)
				throw new KeyNotFoundException($"Task with Id {id} not found.");

			_repo.Delete(id);
		}

		// Mark task as completed
		public void CompleteTask(int id)
		{
			var task = _repo.GetById(id);
			if (task == null)
				throw new KeyNotFoundException($"Task with Id {id} not found.");

			task.MarkCompleted();
			_repo.Update(task);
		}
    }
}
