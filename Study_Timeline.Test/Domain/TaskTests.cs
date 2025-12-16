using Study_Timeline.Logic.Domain;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Test.Domain
{
    public class TaskTests
    {
        private Task CreateTaskForStudent()
        {
            var student = new Student(id: 1, name: "Ivan", password: "pass123");

            // NEW TASK: created via Student
            var task = student.AddTask(
                title: "Study",
                description: "Test"
            );

            // task must have a schedule OR deadline
            task.SetSchedule(DateTime.Now, DateTime.Now.AddHours(1));

            return task;
        }

        [Fact]
        public void MarkCompleted_ShouldSetProgressTo100_AndIsCompletedTrue()
        {
            // Arrange
            var task = CreateTaskForStudent();

            // Act
            task.MarkCompleted();

            // Assert
            Assert.True(task.IsCompleted);
            Assert.Equal(100, task.ProgressPercentage);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(120)]
        public void UpdateProgress_ShouldThrow_WhenPercentageOutsideRange(int invalidValue)
        {
            // Arrange
            var task = CreateTaskForStudent();

            // Act
            Action act = () => task.UpdateProgress(invalidValue);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public void Task_ShouldThrow_WhenNoScheduleOrDeadlineProvided()
        {
            // Arrange
            var task = new Task(
                title: "Study",
                description: "Test"
            );

            // Act
            Action act = () => task.UpdateDetails(
                "Study",
                "Test",
                startTime: null,
                endTime: null,
                deadline: null
            );

            // Assert
            Assert.Throws<InvalidOperationException>(act);
        }
    }
}
