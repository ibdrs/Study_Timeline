using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Services;
using Study_Timeline.Models;

namespace Study_Timeline.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly StudentService service;
        public RegisterModel(StudentService studentService)
        {
            service = studentService;
        }

        [BindProperty]
        public StudentRegistration StudentRegistration { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Student user = new Student(StudentRegistration.UserName, StudentRegistration.Password);
            service.AddStudent(user);

            TempData["Success"] = "Your account has been created successfully. You can now log in.";
            return RedirectToPage("/Auth/Login");
        }
    }
}
