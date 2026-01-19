using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Exceptions;
using Study_Timeline.Logic.Services;
using TaskModel = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.View.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly TaskService _taskService;
        private readonly CategoryService _categoryService;

        public List<TaskModel> Tasks { get; set; } = new();
        private Dictionary<int, Category> _categoryMap = new();

        public IndexModel(TaskService taskService, CategoryService categoryService)
        {
            _taskService = taskService;
            _categoryService = categoryService;
        }

        private int? GetStudentIdOrNull()
        {
            return HttpContext.Session.GetInt32("StudentId");
        }

        public IActionResult OnGet()
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            try
            {
                Tasks = _taskService.GetTasksForStudent(studentId.Value);
                _categoryMap = _categoryService.GetCategoryMapForStudent(studentId.Value);
                return Page();
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToPage("/Auth/Login");
            }
            catch (ForbiddenException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToPage("/Auth/Login");
            }
        }

        public string ResolveCategoryName(TaskModel task)
        {
            if (task.Category == null) return "-";
            if (_categoryMap.TryGetValue(task.Category.Id, out var cat)) return cat.Name;
            return $"Category #{task.Category.Id}";
        }

        public IActionResult OnPostDelete(int id)
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            try
            {
                _taskService.RemoveTaskForStudent(studentId.Value, id);
                TempData["Success"] = "Task deleted.";
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (ForbiddenException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToPage();
        }

        public IActionResult OnPostComplete(int id)
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            try
            {
                _taskService.CompleteTaskForStudent(studentId.Value, id);
                TempData["Success"] = "Task completed.";
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (ForbiddenException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToPage();
        }
    }
}
