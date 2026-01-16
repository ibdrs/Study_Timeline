using Study_Timeline.Logic.Interfaces.Data;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Logic.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repo;
        private readonly IStudentRepository _studentRepo;

        public TaskService(ITaskRepository repo, IStudentRepository studentRepo)
        {
            _repo = repo;
            _studentRepo = studentRepo;
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
        public void UpdateTaskForStudent(int studentId, int taskId, Task updatedTask)
        {
            var task = _repo.GetById(taskId)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!_repo.IsTaskOwnedByStudent(taskId, studentId))
                throw new UnauthorizedAccessException();

            task.UpdateDetails(
                updatedTask.Title,
                updatedTask.Description,
                updatedTask.StartTime,
                updatedTask.EndTime,
                updatedTask.Deadline
            );

            task.UpdateProgress(updatedTask.ProgressPercentage);

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

        public void AddTaskForStudent(int studentId, Task task)
        {
            var student = _studentRepo.GetById(studentId)
                ?? throw new InvalidOperationException("Student not found");

            student.AddTask(task);
            _repo.Add(task, student.Id);
        }

        public void RemoveTaskForStudent(int studentId, int taskId)
        {
            var student = _studentRepo.GetById(studentId)
                ?? throw new InvalidOperationException("Student not found");
            var task = _repo.GetById(taskId)
                ?? throw new InvalidOperationException("Task not found");

            student.RemoveTask(task);
            _repo.Delete(taskId);
        }
    }
}
