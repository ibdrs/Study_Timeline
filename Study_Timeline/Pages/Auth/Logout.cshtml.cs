using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Study_Timeline.View.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            HttpContext.Session.Clear();
        }
    }
}
