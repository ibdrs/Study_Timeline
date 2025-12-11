using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Services;
using Study_Timeline.Models;
using TaskModel = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.View.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly TaskService _taskService;

        [BindProperty]
        public EditTaskInputModel EditTaskInputModel { get; set; } = new();

        public EditModel(TaskService taskService)
        {
            _taskService = taskService;
        }
        public IActionResult OnGet(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
                return NotFound();

            EditTaskInputModel = new EditTaskInputModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartTime = task.StartTime,
                EndTime = task.EndTime,
                ProgressPercentage = task.ProgressPercentage
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            // sanity check to check if there actually is a student
            if (HttpContext.Session.GetInt32("StudentId") == null)
                return RedirectToPage("/Login");

            // get our student id from browser session/cookies
            var studentId = HttpContext.Session.GetInt32("StudentId");

            var task = new TaskModel
            {
                Id = id,
                Title = EditTaskInputModel.Title,
                Description = EditTaskInputModel.Description,
                StartTime = EditTaskInputModel.StartTime,
                EndTime = EditTaskInputModel.EndTime,
                ProgressPercentage = EditTaskInputModel.ProgressPercentage,
                IsCompleted = false
            };

            _taskService.UpdateTask(task);

            return RedirectToPage("Index");
        }

        public IActionResult OnPostComplete(int id)
        {
            _taskService.CompleteTask(id);
            return RedirectToPage("Index");
        }
    }
}
