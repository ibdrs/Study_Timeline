using Study_Timeline.Logic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_Timeline.Logic.Interfaces
{
    public interface IStudentAuthenticationService
    {
        Student? ValidateStudent(string username, string password);
    }
}
