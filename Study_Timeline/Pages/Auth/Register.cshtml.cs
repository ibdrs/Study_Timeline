using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Interfaces;
using Study_Timeline.Logic.Services;
using Study_Timeline.Models;

namespace Study_Timeline.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IStudentRegistrationService _registrationService;
        public RegisterModel(IStudentRegistrationService studentRegistrationService)
        {
            _registrationService = studentRegistrationService;
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
            Student student = new Student(StudentRegistration.UserName, StudentRegistration.Password);
            _registrationService.RegisterStudent(student);

            TempData["RegisterSuccess"] = "Your account has been created successfully. You can now log in.";
            return RedirectToPage("/Auth/Login");
        }
    }
}
