using Study_Timeline.Logic.Exceptions;
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

        public List<Task> GetTasksForStudent(int studentId)
        {
            var student = _studentRepo.GetById(studentId)
                ?? throw new NotFoundException("Student", studentId);

            return _repo.GetByStudentId(student.Id);
        }

        public Task GetTaskForStudent(int taskId, int studentId)
        {
            var task = _repo.GetById(taskId)
                ?? throw new NotFoundException("Task", taskId);

            if (!_repo.IsTaskOwnedByStudent(taskId, studentId))
                throw new ForbiddenException("This task does not belong to you.");

            return task;
        }

        public void UpdateTaskForStudent(int studentId, int taskId, Task updatedTask)
        {
            var task = _repo.GetById(taskId)
                ?? throw new NotFoundException("Task", taskId);

            if (!_repo.IsTaskOwnedByStudent(taskId, studentId))
                throw new ForbiddenException("You are not allowed to edit this task.");

            try
            {
                task.UpdateDetails(
                    updatedTask.Title,
                    updatedTask.Description,
                    updatedTask.StartTime,
                    updatedTask.EndTime,
                    updatedTask.Deadline
                );

                task.UpdateProgress(updatedTask.ProgressPercentage);

                if (updatedTask.Category == null) task.ClearCategory();
                else task.AssignCategory(updatedTask.Category);
            }
            catch (ArgumentException ex)
            {
                throw new ValidationException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new ValidationException(ex.Message);
            }


            _repo.Update(task);
        }

        public void AddTaskForStudent(int studentId, Task task)
        {
            var student = _studentRepo.GetById(studentId)
                ?? throw new NotFoundException("Student", studentId);

            try
            {
                student.AddTask(task);
            }
            catch (ArgumentNullException ex)
            {
                throw new ValidationException(ex.Message);
            }

            _repo.Add(task, student.Id);
        }

        public void CompleteTaskForStudent(int studentId, int taskId)
        {
            var task = _repo.GetById(taskId)
                ?? throw new NotFoundException("Task", taskId);

            if (!_repo.IsTaskOwnedByStudent(taskId, studentId))
                throw new ForbiddenException("You are not allowed to complete this task.");

            task.MarkCompleted();
            _repo.Update(task);
        }


        public void RemoveTaskForStudent(int studentId, int taskId)
        {
            var student = _studentRepo.GetById(studentId)
                ?? throw new NotFoundException("Student", studentId);

            var task = _repo.GetById(taskId)
                ?? throw new NotFoundException("Task", taskId);

            if (!_repo.IsTaskOwnedByStudent(taskId, studentId))
                throw new ForbiddenException("You are not allowed to delete this task.");

            student.RemoveTask(task);
            _repo.Delete(taskId);
        }
    }
}
