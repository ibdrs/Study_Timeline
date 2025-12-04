using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Services;

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
        public LoginInput Input { get; set; } = new();

        public string ErrorMessage { get; set; } = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var student = _studentService.GetStudentByEmail(Input.Email);

            if (student == null || student.Password != Input.Password)
            {
                ErrorMessage = "Invalid email or password.";
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


        public class LoginInput
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }
    }
}
