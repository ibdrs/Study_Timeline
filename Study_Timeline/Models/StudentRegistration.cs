using System.ComponentModel.DataAnnotations;

namespace Study_Timeline.Models
{
    public class StudentRegistration
    {
        [Required(ErrorMessage = "Please fill in a username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }
    }
}
