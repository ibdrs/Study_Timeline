using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Study_Timeline.Logic.Services;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.View.Pages.Tasks
{ 
    public class CreateModel : PageModel
    {
        private readonly TaskService _taskService;

        [BindProperty]
        public CreateTaskInputModel CreateTaskInputModel { get; set; } = new();

        public CreateModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        private static DateTime TrimSeconds(DateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

        public void OnGet()
        {
            CreateTaskInputModel.IsDeadline = true;
            CreateTaskInputModel.StartTime = DateTime.Now;
            CreateTaskInputModel.EndTime = DateTime.Now.AddHours(1);
        }

        public IActionResult OnPost()
        {
            if (CreateTaskInputModel.IsDeadline)
            {
                CreateTaskInputModel.StartTime = null;
                CreateTaskInputModel.EndTime = null;

                if (CreateTaskInputModel.Deadline == null)
                {
                    ModelState.AddModelError(
                        nameof(CreateTaskInputModel.Deadline),
                        "Deadline is required."
                    );
                }
            }
            else
            {
                CreateTaskInputModel.Deadline = null;

                if (CreateTaskInputModel.StartTime == null ||
                    CreateTaskInputModel.EndTime == null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Start time and end time are required."
                    );
                }
                else if (CreateTaskInputModel.EndTime <= CreateTaskInputModel.StartTime)
                {
                    ModelState.AddModelError(
                        nameof(CreateTaskInputModel.EndTime),
                        "End time must be after start time."
                    );
                }
            }

            if (!ModelState.IsValid)
                return Page();

            if (HttpContext.Session.GetInt32("StudentId") == null)
                return RedirectToPage("/Login");

            var studentId = HttpContext.Session.GetInt32("StudentId")!.Value;

            var task = new Task(
                studentId,
                CreateTaskInputModel.Title,
                CreateTaskInputModel.Description
            );

            if (CreateTaskInputModel.IsDeadline)
            {
                task.SetDeadline(
                    TrimSeconds(CreateTaskInputModel.Deadline!.Value)
                );
            }
            else
            {
                task.SetSchedule(
                    TrimSeconds(CreateTaskInputModel.StartTime!.Value),
                    TrimSeconds(CreateTaskInputModel.EndTime!.Value)
                );
            }

            _taskService.AddTask(task);

            return RedirectToPage("Index");
        }
    }
}
