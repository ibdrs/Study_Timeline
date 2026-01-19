using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Exceptions;
using Study_Timeline.Logic.Services;
using Study_Timeline.Models;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.View.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly TaskService _taskService;
        private readonly CategoryService _categoryService;

        public List<Category> Categories { get; private set; } = new();

        private static DateTime TrimSeconds(DateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

        [BindProperty]
        public EditTaskInputModel EditTaskInputModel { get; set; } = new();

        public EditModel(TaskService taskService, CategoryService categoryService)
        {
            _taskService = taskService;
            _categoryService = categoryService;
        }

        private int? GetStudentIdOrNull()
        {
            return HttpContext.Session.GetInt32("StudentId");
        }

        private void LoadCategories(int studentId)
        {
            Categories = _categoryService.GetCategoriesForStudent(studentId);
        }

        public IActionResult OnGet(int id)
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            LoadCategories(studentId.Value);

            try
            {
                var task = _taskService.GetTaskForStudent(id, studentId.Value);

                EditTaskInputModel = new EditTaskInputModel
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    IsDeadline = task.Deadline != null,
                    Deadline = task.Deadline,
                    StartTime = task.StartTime,
                    EndTime = task.EndTime,
                    ProgressPercentage = task.ProgressPercentage,
                    SelectedCategoryId = task.Category?.Id
                };

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
        }

        public IActionResult OnPostAddCategory(int id)
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            LoadCategories(studentId.Value);

            var name = EditTaskInputModel.NewCategoryName?.Trim();
            var desc = EditTaskInputModel.NewCategoryDescription ?? string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError(nameof(EditTaskInputModel.NewCategoryName), "Category name is required.");
                return Page();
            }

            try
            {
                _categoryService.CreateCategoryForStudent(studentId.Value, name, desc);
            }
            catch (ValidationException ex)
            {
                if (!string.IsNullOrWhiteSpace(ex.Field))
                    ModelState.AddModelError($"EditTaskInputModel.{ex.Field}", ex.Message);
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

            LoadCategories(studentId.Value);
            var created = Categories.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            EditTaskInputModel.SelectedCategoryId = created?.Id;

            EditTaskInputModel.NewCategoryName = "";
            EditTaskInputModel.NewCategoryDescription = "";

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            LoadCategories(studentId.Value);

            // UI Validation for time constraints
            if (EditTaskInputModel.IsDeadline)
            {
                EditTaskInputModel.StartTime = null;
                EditTaskInputModel.EndTime = null;

                if (EditTaskInputModel.Deadline == null)
                    ModelState.AddModelError(nameof(EditTaskInputModel.Deadline), "Deadline is required.");
            }
            else
            {
                EditTaskInputModel.Deadline = null;

                if (EditTaskInputModel.StartTime == null || EditTaskInputModel.EndTime == null)
                    ModelState.AddModelError(string.Empty, "Start time and end time are required.");
                else if (EditTaskInputModel.EndTime <= EditTaskInputModel.StartTime)
                    ModelState.AddModelError(nameof(EditTaskInputModel.EndTime), "End time must be after start time.");
            }

            // Validate category selection belongs to student
            if (EditTaskInputModel.SelectedCategoryId.HasValue &&
                !Categories.Any(c => c.Id == EditTaskInputModel.SelectedCategoryId.Value))
            {
                ModelState.AddModelError(nameof(EditTaskInputModel.SelectedCategoryId), "Invalid category selection.");
            }

            if (!ModelState.IsValid)
                return Page();

            var updatedTask = new Task(
                EditTaskInputModel.Title,
                EditTaskInputModel.Description,
                EditTaskInputModel.StartTime == null ? null : TrimSeconds(EditTaskInputModel.StartTime.Value),
                EditTaskInputModel.EndTime == null ? null : TrimSeconds(EditTaskInputModel.EndTime.Value),
                EditTaskInputModel.Deadline == null ? null : TrimSeconds(EditTaskInputModel.Deadline.Value)
            );

            updatedTask.UpdateProgress(EditTaskInputModel.ProgressPercentage);

            if (EditTaskInputModel.SelectedCategoryId.HasValue)
                updatedTask.AssignCategory(new Category(EditTaskInputModel.SelectedCategoryId.Value));
            else
                updatedTask.ClearCategory();

            try
            {
                _taskService.UpdateTaskForStudent(studentId.Value, id, updatedTask);
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

        public IActionResult OnPostComplete(int id)
        {
            var studentId = GetStudentIdOrNull();
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            try
            {
                _taskService.CompleteTaskForStudent(studentId.Value, id);
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
