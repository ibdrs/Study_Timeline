using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Services;
using Study_Timeline.Models;

namespace Study_Timeline.View.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly StudentService _studentService;

        public LoginModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        [BindProperty]
        public StudentLogin StudentLogin { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var student = _studentService.GetStudentByUser(StudentLogin.UserName);

            if (student == null)
            {
                ModelState.AddModelError(string.Empty, "User doesn't exist.");
                return Page();
            }

            // Save to session
            HttpContext.Session.SetInt32("StudentId", student.Id);

            // Save to persistent cookie (30 days)
            HttpContext.Response.Cookies.Append(
                "StudentId",
                student.Id.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(30),
                    HttpOnly = true,
                    Secure = false // set true if using HTTPS
                }
            );

            return RedirectToPage("/Index");
        }
    }
}
