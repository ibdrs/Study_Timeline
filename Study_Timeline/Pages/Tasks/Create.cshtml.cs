using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Services;
using TaskModel = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.View.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly TaskService _taskService;

        [BindProperty]
        public TaskInputModel Input { get; set; } = new();

        public CreateModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var task = new TaskModel
            {
                Title = Input.Title,
                Description = Input.Description,
                StartTime = Input.StartTime,
                EndTime = Input.EndTime,
                ProgressPercentage = 0,
                IsCompleted = false,
                Student = new Student { Id = 1, Name = "Admin", Password = "root" },
                Category = null
            };

            _taskService.AddTask(task);

            return RedirectToPage("Index");
        }

        public class TaskInputModel
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public DateTime StartTime { get; set; } = DateTime.Now;
            public DateTime EndTime { get; set; } = DateTime.Now.AddHours(1);
        }
    }
}
