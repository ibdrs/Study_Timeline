using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Services;
using Study_Timeline.Models;
using Task = Study_Timeline.Logic.Domain.Task;

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
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToPage("/Login");

            var task = _taskService.GetTaskById(id);
            if (task == null || task.StudentId != studentId.Value)
                return NotFound();


            EditTaskInputModel = new EditTaskInputModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartTime = task.StartTime ?? DateTime.Now,
                EndTime = task.EndTime ?? DateTime.Now.AddHours(1),
                ProgressPercentage = task.ProgressPercentage
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToPage("/Login");

            var task = _taskService.GetTaskById(id);
            if (task == null || task.StudentId != studentId.Value)
                return NotFound();

            task.UpdateDetails(
                EditTaskInputModel.Title,
                EditTaskInputModel.Description,
                EditTaskInputModel.StartTime,
                EditTaskInputModel.EndTime,
                null
            );

            task.UpdateProgress(EditTaskInputModel.ProgressPercentage);

            _taskService.UpdateTask(task);

            return RedirectToPage("Index");
        }


        public IActionResult OnPostComplete(int id)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToPage("/Login");

            var task = _taskService.GetTaskById(id);
            if (task == null || task.StudentId != studentId.Value)
                return NotFound();

            _taskService.CompleteTask(id);
            return RedirectToPage("Index");
        }
    }
}
