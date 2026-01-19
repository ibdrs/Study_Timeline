using Moq;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Exceptions;
using Study_Timeline.Logic.Interfaces.Data;
using Study_Timeline.Logic.Services;
using Xunit;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Test.Logic
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepo = new();
        private readonly Mock<IStudentRepository> _studentRepo = new();

        private TaskService CreateService()
        {
            return new TaskService(_taskRepo.Object, _studentRepo.Object);
        }

        [Fact]
        public void GetTasksForStudent_WhenStudentNotFound_ThrowsNotFoundException()
        {
            _studentRepo.Setup(r => r.GetById(1)).Returns((Student?)null);

            var service = CreateService();

            Assert.Throws<NotFoundException>(() => service.GetTasksForStudent(1));

            _taskRepo.Verify(r => r.GetByStudentId(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void GetTaskForStudent_WhenTaskNotFound_ThrowsNotFoundException()
        {
            _taskRepo.Setup(r => r.GetById(10)).Returns((Task?)null);
            var service = CreateService();

            Assert.Throws<NotFoundException>(() => service.GetTaskForStudent(10, 1));
        }

        [Fact]
        public void GetTaskForStudent_WhenNotOwned_ThrowsForbiddenException()
        {
            var task = new Task(
                id: 10,
                title: "T",
                description: "D",
                startTime: null,
                endTime: null,
                deadline: DateTime.Now.AddDays(1),
                progressPercentage: 0,
                isCompleted: false,
                category: null);

            _taskRepo.Setup(r => r.GetById(10)).Returns(task);
            _taskRepo.Setup(r => r.IsTaskOwnedByStudent(10, 1)).Returns(false);

            var service = CreateService();

            Assert.Throws<ForbiddenException>(() => service.GetTaskForStudent(10, 1));
        }

        [Fact]
        public void AddTaskForStudent_WhenStudentNotFound_ThrowsNotFoundException()
        {
            _studentRepo.Setup(r => r.GetById(1)).Returns((Student?)null);

            var service = CreateService();
            var task = new Task("Title", "Desc", null, null, DateTime.Now.AddDays(1));

            Assert.Throws<NotFoundException>(() => service.AddTaskForStudent(1, task));

            _taskRepo.Verify(r => r.Add(It.IsAny<Task>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void AddTaskForStudent_WhenValid_CallsRepoAddWithStudentId()
        {
            var student = new Student(id: 1, name: "ivan", password: "pw");
            _studentRepo.Setup(r => r.GetById(1)).Returns(student);

            var service = CreateService();
            var task = new Task("Title", "Desc", null, null, DateTime.Now.AddDays(1));

            service.AddTaskForStudent(1, task);

            _taskRepo.Verify(r => r.Add(task, 1), Times.Once);
        }

        [Fact]
        public void UpdateTaskForStudent_WhenTaskNotFound_ThrowsNotFoundException()
        {
            _taskRepo.Setup(r => r.GetById(10)).Returns((Task?)null);
            var service = CreateService();

            var updatedTask = new Task("New", "Desc", null, null, DateTime.Now.AddDays(1));

            Assert.Throws<NotFoundException>(() => service.UpdateTaskForStudent(1, 10, updatedTask));

            _taskRepo.Verify(r => r.Update(It.IsAny<Task>()), Times.Never);
        }

        [Fact]
        public void UpdateTaskForStudent_WhenNotOwned_ThrowsForbiddenException()
        {
            var existing = new Task(
                id: 10,
                title: "Old",
                description: "D",
                startTime: null,
                endTime: null,
                deadline: DateTime.Now.AddDays(1),
                progressPercentage: 0,
                isCompleted: false,
                category: null);

            _taskRepo.Setup(r => r.GetById(10)).Returns(existing);
            _taskRepo.Setup(r => r.IsTaskOwnedByStudent(10, 1)).Returns(false);

            var service = CreateService();
            var updatedTask = new Task("New", "Desc", null, null, DateTime.Now.AddDays(1));

            Assert.Throws<ForbiddenException>(() => service.UpdateTaskForStudent(1, 10, updatedTask));

            _taskRepo.Verify(r => r.Update(It.IsAny<Task>()), Times.Never);
        }

        [Fact]
        public void UpdateTaskForStudent_WhenValid_CallsRepoUpdate()
        {
            //arrange
            var existing = new Task(
                id: 10,
                title: "Old",
                description: "D",
                startTime: null,
                endTime: null,
                deadline: DateTime.Now.AddDays(1),
                progressPercentage: 0,
                isCompleted: false,
                category: null);

            _taskRepo.Setup(r => r.GetById(10)).Returns(existing);
            _taskRepo.Setup(r => r.IsTaskOwnedByStudent(10, 1)).Returns(true);

            
            var service = CreateService();

            var updatedTask = new Task("New", "NewDesc", null, null, DateTime.Now.AddDays(2));

            //act
            updatedTask.UpdateProgress(50);

            service.UpdateTaskForStudent(1, 10, updatedTask);

            //assert
            _taskRepo.Verify(r => r.Update(existing), Times.Once);
        }

        [Fact]
        public void RemoveTaskForStudent_WhenNotOwned_ThrowsForbiddenException()
        {
            //arrange
            var student = new Student(id: 1, name: "ivan", password: "pw");
            var task = new Task(
                id: 10,
                title: "T",
                description: "D",
                startTime: null,
                endTime: null,
                deadline: DateTime.Now.AddDays(1),
                progressPercentage: 0,
                isCompleted: false,
                category: null);

            _studentRepo.Setup(r => r.GetById(1)).Returns(student);
            _taskRepo.Setup(r => r.GetById(10)).Returns(task);
            _taskRepo.Setup(r => r.IsTaskOwnedByStudent(10, 1)).Returns(false);

            var service = CreateService();

            //act + assert
            Assert.Throws<ForbiddenException>(() => service.RemoveTaskForStudent(1, 10));

            _taskRepo.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RemoveTaskForStudent_WhenOwned_DeletesTask()
        {
            //arrange
            var student = new Student(id: 1, name: "ivan", password: "pw");
            var task = new Task(
                id: 10,
                title: "T",
                description: "D",
                startTime: null,
                endTime: null,
                deadline: DateTime.Now.AddDays(1),
                progressPercentage: 0,
                isCompleted: false,
                category: null);

            _studentRepo.Setup(r => r.GetById(1)).Returns(student);
            _taskRepo.Setup(r => r.GetById(10)).Returns(task);
            _taskRepo.Setup(r => r.IsTaskOwnedByStudent(10, 1)).Returns(true);

            var service = CreateService();

            //act
            service.RemoveTaskForStudent(1, 10);
            
            //assert
            _taskRepo.Verify(r => r.Delete(10), Times.Once);
        }
    }

}