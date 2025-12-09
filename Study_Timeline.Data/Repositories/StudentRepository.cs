using Microsoft.Data.SqlClient;
using Study_Timeline.Logic.Domain;
using Study_Timeline.Logic.Interfaces;

namespace Study_Timeline.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbConnectionFactory _factory;

        public StudentRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public Student? GetByUser(string user)
        {
            using var connection = _factory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT Id, Name, Password FROM Students WHERE Name=@Name",
                connection
            );

            command.Parameters.AddWithValue("@Name", user);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (!reader.Read()) return null;

            return new Student
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString()!,
                Password = reader["Password"].ToString()!,
            };
        }

        public void Add(Student student)
        {
            using var connection = _factory.CreateConnection();
            using var command = new SqlCommand(
                "INSERT INTO Students (Name, Password) VALUES (@Name, @Password)",
                connection
            );

            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Password", student.Password);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
