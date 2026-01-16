using Moq;
using Study_Timeline.Logic.Services;
using Study_Timeline.Logic.Domain;
using Task = Study_Timeline.Logic.Domain.Task;
using Study_Timeline.Logic.Interfaces.Data;

public class TaskServiceTests
{
    private const int StudentId = 1;

    private Task CreateTaskForStudent(string title)
    {
        var student = new Student(StudentId, "Ivan", "pass123");

        // task must have a schedule OR deadline
        var task = new Task(
            title: title,
            description: "Test",
            startTime: DateTime.Now,
            endTime: DateTime.Now.AddHours(1),
            deadline: null
        );

        student.AddTask(task);

        return task;
    }

    // Test 1 - UpdateTask moet repo aanroepen wanneer geldig
    [Fact]
    public void UpdateTaskForStudent_Should_Update_Task_When_Valid()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        var service = new TaskService(mockRepo.Object);

        var task = CreateTaskForStudent("Homework");

        mockRepo.Setup(r => r.GetById(task.Id)).Returns(task);
        mockRepo.Setup(r => r.IsTaskOwnedByStudent(task.Id, StudentId)).Returns(true);

        // Act
        var updatedTask = new Task(
            title: "Homework updated",
            description: "Updated desc",
            startTime: task.StartTime,
            endTime: task.EndTime,
            deadline: null
        );

        updatedTask.UpdateProgress(50);

        service.UpdateTaskForStudent(StudentId, task.Id, updatedTask);

        // Assert
        mockRepo.Verify(r => r.Update(task), Times.Once);
        Assert.Equal(50, task.ProgressPercentage);
    }

    // Test 2 — AddTask moet fout geven zonder titel
    [Fact]
    public void AddTask_Should_Throw_When_Title_Is_Empty()
    {
        // Arrange + Act + Assert
        // empty title , should throw argument exception
        Assert.Throws<ArgumentException>(() =>
            CreateTaskForStudent("")
        );
    }

    // Test 3 — CompleteTask moet task ophalen, status wijzigen en opslaan
    [Fact]
    public void CompleteTaskForStudent_Should_Mark_Task_As_Completed_And_Update_Repo()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        var task = CreateTaskForStudent("Homework");

        mockRepo.Setup(r => r.GetById(task.Id)).Returns(task);
        mockRepo.Setup(r => r.IsTaskOwnedByStudent(task.Id, StudentId)).Returns(true);

        var service = new TaskService(mockRepo.Object);

        // Act
        service.CompleteTaskForStudent(StudentId, task.Id);

        // Assert
        Assert.True(task.IsCompleted);
        Assert.Equal(100, task.ProgressPercentage);
        mockRepo.Verify(r => r.Update(task), Times.Once);
    }

    // Test 4 — CompleteTask moet fout geven bij ongeldig ID
    [Fact]
    public void CompleteTask_Should_Throw_When_Task_Not_Found()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        mockRepo.Setup(r => r.GetById(10)).Returns((Task?)null);

        var service = new TaskService(mockRepo.Object);

        // Act + Assert
        Assert.Throws<KeyNotFoundException>(() =>
            service.CompleteTaskForStudent(StudentId, 10)
        );
    }

    // Test 5 — UpdateTask mag niet als student geen eigenaar is
    [Fact]
    public void UpdateTask_Should_Throw_When_Task_Not_Owned_By_Student()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        var service = new TaskService(mockRepo.Object);

        var task = CreateTaskForStudent("Homework");

        mockRepo.Setup(r => r.GetById(task.Id)).Returns(task);
        mockRepo.Setup(r => r.IsTaskOwnedByStudent(task.Id, StudentId)).Returns(false);

        // Act + Assert
        Assert.Throws<UnauthorizedAccessException>(() =>
            service.UpdateTaskForStudent(
                StudentId,
                task.Id,
                new Task(
                    title: "X",
                    description: "Y",
                    startTime: task.StartTime,
                    endTime: task.EndTime,
                    deadline: null
                )
            )
        );
    }
}
