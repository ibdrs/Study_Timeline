using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Services;
using Study_Timeline.Models;
using TaskModel = Study_Timeline.Logic.Domain.Task;

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

        public void OnGet() { } 
        public IActionResult OnPost()
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
                Title = CreateTaskInputModel.Title,
                Description = CreateTaskInputModel.Description,
                StartTime = CreateTaskInputModel.StartTime,
                EndTime = CreateTaskInputModel.EndTime,
                ProgressPercentage = 0,
                IsCompleted = false,
                Student = new Student(id: studentId.Value),
                Category = null
            };

            _taskService.AddTask(task);

            return RedirectToPage("Index");
        }
    }
}
