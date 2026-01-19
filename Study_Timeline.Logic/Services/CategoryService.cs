using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Exceptions;
using Study_Timeline.Logic.Interfaces.Data;

namespace Study_Timeline.Logic.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public List<Category> GetCategoriesForStudent(int studentId)
        {
            return _repo.GetByStudentId(studentId);
        }

        public Dictionary<int, Category> GetCategoryMapForStudent(int studentId)
        {
            return _repo.GetMapByStudentId(studentId);
        }

        public void CreateCategoryForStudent(int studentId, string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Category name is required.", field: "NewCategoryName");

            if (_repo.ExistsByName(studentId, name))
                throw new ValidationException("Category name must be unique.", field: "NewCategoryName");

            var category = new Category(id: 0, name: name, description: description);
            _repo.Add(category, studentId);
        }

        public void DeleteCategoryForStudent(int studentId, int categoryId)
        {
            _repo.Delete(categoryId, studentId);
        }
    }
}
