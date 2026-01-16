using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Interfaces;
using Study_Timeline.Logic.Services;
using Study_Timeline.Models.Timeline;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TaskService _taskService;
        private readonly CategoryService _categoryService;
        private readonly ITimelineBuilder _timelineBuilder;
        private readonly IProgressCalculator _progressCalculator;

        public TimelinePageVM Timeline { get; private set; } = new();

        public IndexModel(
            TaskService taskService,
            CategoryService categoryService,
            ITimelineBuilder timelineBuilder,
            IProgressCalculator progressCalculator)
        {
            _taskService = taskService;
            _categoryService = categoryService;
            _timelineBuilder = timelineBuilder;
            _progressCalculator = progressCalculator;
        }

        public IActionResult OnGet()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
                return RedirectToPage("/Auth/Login");

            var tasks = _taskService.GetTasksForStudent(studentId.Value);
            var categoryMap = _categoryService.GetCategoryMapForStudent(studentId.Value);

            // Build weekly groups
            var weeks = _timelineBuilder.BuildWeekly(tasks);

            Timeline = new TimelinePageVM
            {
                Weeks = weeks.Select(w => new TimelineWeekVM
                {
                    WeekStart = w.WeekStart,
                    WeekEnd = w.WeekEnd,
                    IsoWeekNumber = w.IsoWeekNumber,
                    IsoWeekYear = w.IsoWeekYear,
                    ProgressPercent = _progressCalculator.CalculateWeekProgressPercent(w),
                    Tasks = w.Tasks.Select(e => new TimelineTaskItemVM
                    {
                        Id = e.Task.Id,
                        Title = e.Task.Title,
                        Description = e.Task.Description,
                        Anchor = e.Anchor,
                        StartTime = e.Task.StartTime,
                        EndTime = e.Task.EndTime,
                        Deadline = e.Task.Deadline,
                        IsCompleted = e.Task.IsCompleted,
                        ProgressPercentage = e.Task.ProgressPercentage,
                        DurationMinutes = e.DurationMinutes,
                        CategoryName = ResolveCategoryName(e.Task, categoryMap)
                    }).ToList()
                }).ToList()
            };

            return Page();
        }

        private static string? ResolveCategoryName(Task task, IReadOnlyDictionary<int, Category> categoryMap)
        {
            if (task.Category == null)
                return null;

            if (categoryMap.TryGetValue(task.Category.Id, out var category))
                return category.Name;

            return $"Category #{task.Category.Id}";
        }
    }
}
