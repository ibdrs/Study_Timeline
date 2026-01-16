using Study_Timeline.Logic.Domain;

namespace Study_Timeline.Logic.Interfaces.Data
{
    public interface ICategoryRepository
    {
        Category? GetById(int id);
        List<Category> GetByStudentId(int studentId);
        Dictionary<int, Category> GetMapByStudentId(int studentId);
        void Add(Category category, int studentId);
        void Update(Category category, int studentId);
        void Delete(int categoryId, int studentId);
        bool ExistsByName(int studentId, string name);
    }
}
