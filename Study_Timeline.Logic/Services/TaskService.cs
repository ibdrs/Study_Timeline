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

        public List<Task> GetTasksForStudent(int studentId)
        {
            return _repo.GetByStudentId(studentId);
        }

        public Task? GetTaskForStudent(int taskId, int studentId)
        {
            var task = _repo.GetById(taskId);
            if (task == null)
                return null;

            if (!_repo.IsTaskOwnedByStudent(taskId, studentId))
                return null;

            return task;
        }

        // Update an existing task
        public void UpdateTaskForStudent(
            int studentId,
            int taskId,
            string title,
            string description,
            DateTime? start,
            DateTime? end,
            DateTime? deadline,
            int progress)
        {
            var task = _repo.GetById(taskId)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!_repo.IsTaskOwnedByStudent(taskId, studentId))
                throw new UnauthorizedAccessException();

            if (deadline == null && (start == null || end == null))
                throw new InvalidOperationException(
                    "Task must have either a deadline or a start and end time."
                );

            if (deadline.HasValue)
            {
                task.UpdateDetails(
                    title,
                    description,
                    null,
                    null,
                    TrimSeconds(deadline.Value)
                );
            }
            else
            {
                task.UpdateDetails(
                    title,
                    description,
                    TrimSeconds(start!.Value),
                    TrimSeconds(end!.Value),
                    null
                );
            }

            task.UpdateProgress(progress);

            _repo.Update(task);
        }


        // Delete a task
        public void DeleteTaskForStudent(int studentId, int taskId)
        {
            var task = _repo.GetById(taskId)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!_repo.IsTaskOwnedByStudent(taskId, studentId))
                throw new UnauthorizedAccessException();

            _repo.Delete(taskId);
        }

        // Mark task as completed
        public void CompleteTaskForStudent(int studentId, int taskId)
        {
            var task = _repo.GetById(taskId)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!_repo.IsTaskOwnedByStudent(taskId, studentId))
                throw new UnauthorizedAccessException();

            task.MarkCompleted();
            _repo.Update(task);
        }

        private static DateTime TrimSeconds(DateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
    }
}
