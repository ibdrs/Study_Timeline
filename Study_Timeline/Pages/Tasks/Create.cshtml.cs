using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Study_Timeline.Logic.Services;


namespace Study_Timeline.View.Pages.Tasks
{ 
    public class CreateModel : PageModel
    {
        private readonly StudentService _studentService;

        [BindProperty]
        public CreateTaskInputModel CreateTaskInputModel { get; set; } = new();

        public CreateModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        private static DateTime TrimSeconds(DateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

        public void OnGet()
        {
            CreateTaskInputModel.IsDeadline = true;
            CreateTaskInputModel.StartTime = DateTime.Now;
            CreateTaskInputModel.EndTime = DateTime.Now.AddHours(1);
        }

        public IActionResult OnPost()
        {
            if (CreateTaskInputModel.IsDeadline)
            {
                CreateTaskInputModel.StartTime = null;
                CreateTaskInputModel.EndTime = null;

                if (CreateTaskInputModel.Deadline == null)
                {
                    ModelState.AddModelError(
                        nameof(CreateTaskInputModel.Deadline),
                        "Deadline is required."
                    );
                }
            }
            else
            {
                CreateTaskInputModel.Deadline = null;

                if (CreateTaskInputModel.StartTime == null ||
                    CreateTaskInputModel.EndTime == null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Start time and end time are required."
                    );
                }
                else if (CreateTaskInputModel.EndTime <= CreateTaskInputModel.StartTime)
                {
                    ModelState.AddModelError(
                        nameof(CreateTaskInputModel.EndTime),
                        "End time must be after start time."
                    );
                }
            }

            if (!ModelState.IsValid)
                return Page();

            if (HttpContext.Session.GetInt32("StudentId") == null)
                return RedirectToPage("/Login");

            var studentId = HttpContext.Session.GetInt32("StudentId")!.Value;

            _studentService.AddTaskForStudent(
                studentId,
                CreateTaskInputModel.Title,
                CreateTaskInputModel.Description,
                CreateTaskInputModel.StartTime,
                CreateTaskInputModel.EndTime,
                CreateTaskInputModel.Deadline
            );

            return RedirectToPage("Index");
        }
    }
}
