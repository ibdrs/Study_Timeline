using Study_Timeline.Logic.Domain;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Test.Domain
{
    public class TaskTests
    {
        private Task CreateTask()
        {
            return new Task
            {
                Id = 1,
                Title = "Study",
                Description = "Test",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                Student = new Student(id: 1)
            };
        }

        // happy scenario (task is completed)
        [Fact]
        public void MarkCompleted_ShouldSetProgressTo100_AndIsCompletedTrue()
        {
            // Arrange
            var task = CreateTask();

            // Act
            task.MarkCompleted();

            // Assert
            Assert.True(task.IsCompleted);
            Assert.Equal(100, task.ProgressPercentage);
        }

        // unhappy scenario (out of range)
        [Theory]
        [InlineData(-5)]
        [InlineData(120)]
        public void UpdateProgress_ShouldThrow_WhenPercentageOutsideRange(int invalidValue)
        {
            // Arrange
            var task = CreateTask();

            // Act
            Action act = () => task.UpdateProgress(invalidValue);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(act);
        }
    }
}