using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Services;
using Study_Timeline.Models;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.View.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly TaskService _taskService;

        private static DateTime TrimSeconds(DateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

        [BindProperty]
        public EditTaskInputModel EditTaskInputModel { get; set; } = new();

        public EditModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public IActionResult OnGet(int id)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            var task = _taskService.GetTaskForStudent(id, studentId.Value);
            if (task == null)
                return NotFound();

            EditTaskInputModel = new EditTaskInputModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsDeadline = task.Deadline != null,
                Deadline = task.Deadline,
                StartTime = task.StartTime,
                EndTime = task.EndTime,
                ProgressPercentage = task.ProgressPercentage
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            // UI Validation for time constraints
            if (EditTaskInputModel.IsDeadline)
            {
                EditTaskInputModel.StartTime = null;
                EditTaskInputModel.EndTime = null;

                if (EditTaskInputModel.Deadline == null)
                {
                    ModelState.AddModelError(
                        nameof(EditTaskInputModel.Deadline),
                        "Deadline is required."
                    );
                }
            }
            else
            {
                EditTaskInputModel.Deadline = null;

                if (EditTaskInputModel.StartTime == null || EditTaskInputModel.EndTime == null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Start time and end time are required."
                    );
                }
                else if (EditTaskInputModel.EndTime <= EditTaskInputModel.StartTime)
                {
                    ModelState.AddModelError(
                        nameof(EditTaskInputModel.EndTime),
                        "End time must be after start time."
                    );
                }
            }

            if (!ModelState.IsValid)
                return Page();

            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            var updatedTask = new Task(
                EditTaskInputModel.Title,
                EditTaskInputModel.Description,
                EditTaskInputModel.StartTime == null ? null : TrimSeconds(EditTaskInputModel.StartTime.Value),
                EditTaskInputModel.EndTime == null ? null : TrimSeconds(EditTaskInputModel.EndTime.Value),
                EditTaskInputModel.Deadline == null ? null : TrimSeconds(EditTaskInputModel.Deadline.Value)
            );

            updatedTask.UpdateProgress(EditTaskInputModel.ProgressPercentage);

            _taskService.UpdateTaskForStudent(studentId.Value, id, updatedTask);

            return RedirectToPage("Index");
        }

        public IActionResult OnPostComplete(int id)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            _taskService.CompleteTaskForStudent(studentId.Value, id);

            return RedirectToPage("Index");
        }
    }
}
