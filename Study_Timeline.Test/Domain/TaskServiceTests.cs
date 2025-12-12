using Moq;
using Study_Timeline.Logic.Interfaces;
using Study_Timeline.Logic.Services;
using Study_Timeline.Logic.Domain;
using Task = Study_Timeline.Logic.Domain.Task;

public class TaskServiceTests
{
    private Task CreateTaskForStudent(string title = "Homework")
    {
        var student = new Student(1, "Ivan", "pass123");

        // NEW TASK: no id, no schedule yet
        var task = new Task(
            title: title,
            description: "Test"
        );

        // enforce invariant: task must have a schedule OR deadline
        task.SetSchedule(DateTime.Now, DateTime.Now.AddHours(1));

        // Add the task so it belongs to this student
        student.AddTask(task);

        return task;
    }

    // Test 1 - AddTask moet validatie doen + repo aanroepen
    [Fact]
    public void AddTask_Should_Add_Task_When_Valid()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        var service = new TaskService(mockRepo.Object);

        var task = CreateTaskForStudent("Homework");

        // Act
        service.AddTask(task);

        // Assert
        mockRepo.Verify(r => r.Add(task), Times.Once);
    }

    // Test 2 — AddTask moet fout geven zonder titel
    [Fact]
    public void AddTask_Should_Throw_When_Title_Is_Empty()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        var service = new TaskService(mockRepo.Object);

        // empty title , should throw argument exception
        Assert.Throws<ArgumentException>(() =>
            CreateTaskForStudent("")
        );
    }

    // Test 3 — CompleteTask moet task ophalen, status wijzigen en opslaan
    [Fact]
    public void CompleteTask_Should_Mark_Task_As_Completed_And_Update_Repo()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        var task = CreateTaskForStudent();

        mockRepo.Setup(r => r.GetById(task.Id)).Returns(task);

        var service = new TaskService(mockRepo.Object);

        // Act
        service.CompleteTask(task.Id);

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
        Assert.Throws<KeyNotFoundException>(() => service.CompleteTask(10));
    }

    // Test 5 — UpdateTask mag niet als IsCompleted = true
    [Fact]
    public void UpdateTask_Should_Throw_When_Task_Is_Completed()
    {
        // Arrange
        var mockRepo = new Mock<ITaskRepository>();
        var service = new TaskService(mockRepo.Object);

        var task = CreateTaskForStudent();

        // complete the task
        task.MarkCompleted();

        mockRepo.Setup(r => r.GetById(task.Id)).Returns(task);

        // Act + Assert
        Assert.Throws<InvalidOperationException>(() => service.UpdateTask(task));
    }
}
