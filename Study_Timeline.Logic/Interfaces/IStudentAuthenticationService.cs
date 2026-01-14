using Study_Timeline.Logic.Domain;

namespace Study_Timeline.Logic.Interfaces
{
    public interface IStudentAuthenticationService
    {
        Student? ValidateStudent(string username, string password);
    }
}
