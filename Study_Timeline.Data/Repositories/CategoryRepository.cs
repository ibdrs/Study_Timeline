using Microsoft.Data.SqlClient;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Interfaces.Data;

namespace Study_Timeline.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbConnectionFactory _factory;

        public CategoryRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public Category? GetById(int id)
        {
            using var connection = _factory.CreateConnection();
            using var command = new SqlCommand("SELECT TOP 1 * FROM Categories WHERE Id=@Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;

            return new Category(
                id: (int)reader["Id"],
                name: reader["Name"].ToString()!,
                description: reader["Description"].ToString()!
            );
        }

        public List<Category> GetByStudentId(int studentId)
        {
            var result = new List<Category>();

            using var connection = _factory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT * FROM Categories WHERE StudentId=@StudentId ORDER BY Name",
                connection);
            command.Parameters.AddWithValue("@StudentId", studentId);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Category(
                    id: (int)reader["Id"],
                    name: reader["Name"].ToString()!,
                    description: reader["Description"].ToString()!
                ));
            }

            return result;
        }

        public Dictionary<int, Category> GetMapByStudentId(int studentId)
        {
            return GetByStudentId(studentId)
                .ToDictionary(c => c.Id, c => c);
        }

        public void Add(Category category, int studentId)
        {
            var query = @"INSERT INTO Categories (Name, Description, StudentId)
                           VALUES (@Name, @Description, @StudentId)";

            using var connection = _factory.CreateConnection();
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", category.Name);
            command.Parameters.AddWithValue("@Description", category.Description ?? string.Empty);
            command.Parameters.AddWithValue("@StudentId", studentId);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Update(Category category, int studentId)
        {
            var query = @"UPDATE Categories SET Name=@Name, Description=@Description
                           WHERE Id=@Id AND StudentId=@StudentId";

            using var connection = _factory.CreateConnection();
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", category.Id);
            command.Parameters.AddWithValue("@Name", category.Name);
            command.Parameters.AddWithValue("@Description", category.Description ?? string.Empty);
            command.Parameters.AddWithValue("@StudentId", studentId);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Delete(int categoryId, int studentId)
        {
            // Requirement C-07.4: tasks assigned to a deleted category must revert to having no category.
            using var connection = _factory.CreateConnection();
            connection.Open();

            using var tx = connection.BeginTransaction();

            try
            {
                using (var clear = new SqlCommand(
                           "UPDATE Tasks SET CategoryId = NULL WHERE CategoryId = @CategoryId AND StudentId=@StudentId",
                           connection, tx))
                {
                    clear.Parameters.AddWithValue("@CategoryId", categoryId);
                    clear.Parameters.AddWithValue("@StudentId", studentId);
                    clear.ExecuteNonQuery();
                }

                using (var del = new SqlCommand(
                           "DELETE FROM Categories WHERE Id=@CategoryId AND StudentId=@StudentId",
                           connection, tx))
                {
                    del.Parameters.AddWithValue("@CategoryId", categoryId);
                    del.Parameters.AddWithValue("@StudentId", studentId);
                    del.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public bool ExistsByName(int studentId, string name)
        {
            using var connection = _factory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT COUNT(*) FROM Categories WHERE StudentId=@StudentId AND Name=@Name",
                connection);
            command.Parameters.AddWithValue("@StudentId", studentId);
            command.Parameters.AddWithValue("@Name", name);

            connection.Open();
            return (int)command.ExecuteScalar() > 0;
        }
    }
}
