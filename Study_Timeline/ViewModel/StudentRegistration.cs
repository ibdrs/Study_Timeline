using System.ComponentModel.DataAnnotations;

namespace Study_Timeline.ViewModel
{
    public class StudentRegistration
    {
        [Required(ErrorMessage = "Please fill in a username")]
        public string UserName;
        [Required(ErrorMessage = "Please enter a password")]

        public string Password;
    }
}
