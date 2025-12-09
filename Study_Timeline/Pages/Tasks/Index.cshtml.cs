using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Services;
using TaskModel = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.View.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly TaskService _taskService;

        public List<TaskModel> Tasks { get; set; } = new();

        public IndexModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public void OnGet()
        {
            Tasks = _taskService.GetAllTasks();
        }

        public IActionResult OnPostDelete(int id)
        {
            _taskService.DeleteTask(id);
            return RedirectToPage();
        }

        public IActionResult OnPostComplete(int id)
        {
            _taskService.CompleteTask(id);
            return RedirectToPage();
        }
        public IActionResult OnPostEdit(int id)
        {
            var Task = _taskService.GetTaskById(id);

            if (Task == null) {
                ModelState.AddModelError("", "Task not found.");
                return Page();
            }

            _taskService.UpdateTask(Task);
            return RedirectToPage("Edit");
        }
    }
}
