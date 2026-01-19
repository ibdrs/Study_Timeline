using Moq;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Exceptions;
using Study_Timeline.Logic.Interfaces.Data;
using Study_Timeline.Logic.Services;
using Xunit;

namespace Study_Timeline.Test.Logic
{
    public class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _studentRepo = new();
        private readonly Mock<ITaskRepository> _taskRepo = new();

        private StudentService CreateService()
        {
            return new StudentService(_studentRepo.Object, _taskRepo.Object);
        }

        [Fact]
        public void RegisterStudent_WhenUsernameAlreadyExists_ThrowsValidationException()
        {
            var existing = new Student(id: 1, name: "ivan", password: "pw");
            _studentRepo.Setup(r => r.GetByUser("ivan")).Returns(existing);

            var service = CreateService();
            var newStudent = new Student(name: "ivan", password: "pw2");

            Assert.Throws<ValidationException>(() => service.RegisterStudent(newStudent));

            _studentRepo.Verify(r => r.Add(It.IsAny<Student>()), Times.Never);
        }

        [Fact]
        public void RegisterStudent_WhenFree_AddsStudent()
        {
            _studentRepo.Setup(r => r.GetByUser("newuser")).Returns((Student?)null);

            var service = CreateService();
            var student = new Student(name: "newuser", password: "pw");

            service.RegisterStudent(student);

            _studentRepo.Verify(r => r.Add(student), Times.Once);
        }
    }

}