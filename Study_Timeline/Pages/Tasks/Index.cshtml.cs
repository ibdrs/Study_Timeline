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

        public IActionResult OnGet()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToPage("/Login");

            Tasks = _taskService.GetTasksForStudent(studentId.Value);
            return Page();
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
    }
}
