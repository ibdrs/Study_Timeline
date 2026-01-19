using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Exceptions;
using Study_Timeline.Logic.Services;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.View.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly TaskService _taskService;
        private readonly CategoryService _categoryService;

        public List<Category> Categories { get; private set; } = new();

        [BindProperty]
        public CreateTaskInputModel CreateTaskInputModel { get; set; } = new();

        public CreateModel(TaskService taskService, CategoryService categoryService)
        {
            _taskService = taskService;
            _categoryService = categoryService;
        }

        private static DateTime TrimSeconds(DateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

        private int? GetStudentIdOrNull()
        {
            return HttpContext.Session.GetInt32("StudentId");
        }

        private void LoadCategories(int studentId)
        {
            Categories = _categoryService.GetCategoriesForStudent(studentId);
        }

        public IActionResult OnGet()
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            LoadCategories(studentId.Value);

            CreateTaskInputModel.IsDeadline = true;
            CreateTaskInputModel.StartTime = DateTime.Now;
            CreateTaskInputModel.EndTime = DateTime.Now.AddHours(1);
            CreateTaskInputModel.SelectedCategoryId = null;

            return Page();
        }

        // POST: Add Category (inline)
        public IActionResult OnPostAddCategory()
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            LoadCategories(studentId.Value);

            var name = CreateTaskInputModel.NewCategoryName?.Trim();
            var desc = CreateTaskInputModel.NewCategoryDescription ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError(nameof(CreateTaskInputModel.NewCategoryName), "Category name is required.");
                return Page();
            }

            try
            {
                _categoryService.CreateCategoryForStudent(studentId.Value, name, desc);
            }
            catch (ValidationException ex)
            {
                if (!string.IsNullOrWhiteSpace(ex.Field))
                    ModelState.AddModelError($"CreateTaskInputModel.{ex.Field}", ex.Message);
                else
                    ModelState.AddModelError(string.Empty, ex.Message);

                return Page();
            }
            catch (ForbiddenException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToPage("Index");
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToPage("Index");
            }

            // reload + auto-select newly created category
            LoadCategories(studentId.Value);
            var created = Categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            CreateTaskInputModel.SelectedCategoryId = created?.Id;

            // Clear inline inputs
            CreateTaskInputModel.NewCategoryName = "";
            CreateTaskInputModel.NewCategoryDescription = "";

            // Keep sensible defaults for the task fields when returning to the page
            if (CreateTaskInputModel.StartTime == null) CreateTaskInputModel.StartTime = DateTime.Now;
            if (CreateTaskInputModel.EndTime == null) CreateTaskInputModel.EndTime = DateTime.Now.AddHours(1);
            if (CreateTaskInputModel.IsDeadline == false && CreateTaskInputModel.Deadline != null)
                CreateTaskInputModel.Deadline = null;

            return Page();
        }

        public IActionResult OnPost()
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            LoadCategories(studentId.Value);

            // UI validation for time constraints
            if (CreateTaskInputModel.IsDeadline)
            {
                CreateTaskInputModel.StartTime = null;
                CreateTaskInputModel.EndTime = null;

                if (CreateTaskInputModel.Deadline == null)
                    ModelState.AddModelError(nameof(CreateTaskInputModel.Deadline), "Deadline is required.");
            }
            else
            {
                CreateTaskInputModel.Deadline = null;

                if (CreateTaskInputModel.StartTime == null || CreateTaskInputModel.EndTime == null)
                    ModelState.AddModelError(string.Empty, "Start time and end time are required.");
                else if (CreateTaskInputModel.EndTime <= CreateTaskInputModel.StartTime)
                    ModelState.AddModelError(nameof(CreateTaskInputModel.EndTime), "End time must be after start time.");
            }

            // Validate selected category belongs to student
            if (CreateTaskInputModel.SelectedCategoryId.HasValue &&
                !Categories.Any(c => c.Id == CreateTaskInputModel.SelectedCategoryId.Value))
            {
                ModelState.AddModelError(nameof(CreateTaskInputModel.SelectedCategoryId), "Invalid category selection.");
            }

            if (!ModelState.IsValid)
                return Page();

            var task = new Task(
                CreateTaskInputModel.Title,
                CreateTaskInputModel.Description,
                CreateTaskInputModel.StartTime == null ? null : TrimSeconds(CreateTaskInputModel.StartTime.Value),
                CreateTaskInputModel.EndTime == null ? null : TrimSeconds(CreateTaskInputModel.EndTime.Value),
                CreateTaskInputModel.Deadline == null ? null : TrimSeconds(CreateTaskInputModel.Deadline.Value)
            );

            if (CreateTaskInputModel.SelectedCategoryId.HasValue)
                task.AssignCategory(new Category(CreateTaskInputModel.SelectedCategoryId.Value));

            try
            {
                _taskService.AddTaskForStudent(studentId.Value, task);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (ForbiddenException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToPage("Index");
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToPage("Index");
            }

            return RedirectToPage("Index");
        }
    }
}
