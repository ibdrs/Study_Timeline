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

        public Student? GetByEmail(string email)
        {
            using var connection = _factory.CreateConnection();
            using var command = new SqlCommand(
                "SELECT Id, Name, Email, Password FROM Students WHERE Email=@Email",
                connection
            );

            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (!reader.Read()) return null;

            return new Student
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString()!,
                Email = reader["Email"].ToString()!,
                Password = reader["Password"].ToString()!,
                Categories = new(),
                Tasks = new()
            };
        }

        public void Add(Student student)
        {
            using var connection = _factory.CreateConnection();
            using var command = new SqlCommand(
                @"INSERT INTO Students (Name, Email, Password)
                  VALUES (@Name, @Email, @Password)",
                connection
            );

            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Email", student.Email);
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
