using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Study_Timeline.View.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();

            HttpContext.Response.Cookies.Delete("StudentId");

            return RedirectToPage("/Auth/Login");
        }
    }
}
