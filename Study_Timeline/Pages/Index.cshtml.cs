using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Study_Timeline.Data.Repositories;
using Study_Timeline.Logic.Services;

namespace Study_Timeline.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("StudentId") == null)
                return RedirectToPage("/Auth/Login");

            return Page();
        }

    }
}
