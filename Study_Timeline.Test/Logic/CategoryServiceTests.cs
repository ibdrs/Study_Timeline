using Moq;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Exceptions;
using Study_Timeline.Logic.Interfaces.Data;
using Study_Timeline.Logic.Services;
using Xunit;

namespace Study_Timeline.Test.Logic
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _repo = new();

        private CategoryService CreateService()
        {
            return new CategoryService(_repo.Object);
        }

        [Fact]
        public void CreateCategoryForStudent_WhenNameEmpty_ThrowsValidationException()
        {
            var service = CreateService();

            Assert.Throws<ValidationException>(() =>
                service.CreateCategoryForStudent(1, "   ", "desc"));

            _repo.Verify(r => r.Add(It.IsAny<Category>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void CreateCategoryForStudent_WhenNameExists_ThrowsValidationException()
        {
            _repo.Setup(r => r.ExistsByName(1, "School")).Returns(true);
            var service = CreateService();

            Assert.Throws<ValidationException>(() =>
                service.CreateCategoryForStudent(1, "School", "desc"));

            _repo.Verify(r => r.Add(It.IsAny<Category>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void CreateCategoryForStudent_WhenValid_CallsRepoAdd()
        {
            _repo.Setup(r => r.ExistsByName(1, "Work")).Returns(false);
            var service = CreateService();

            service.CreateCategoryForStudent(1, "Work", "desc");

            _repo.Verify(r => r.Add(It.Is<Category>(c => c.Name == "Work"), 1), Times.Once);
        }
    }

}