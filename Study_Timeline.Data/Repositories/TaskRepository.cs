using Microsoft.Data.SqlClient;
using Study_Timeline.Logic.Interfaces;
using Task = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Data.Repositories
{
	public class TaskRepository : ITaskRepository
	{
		private readonly DbConnectionFactory _factory;

		public TaskRepository(DbConnectionFactory factory)
		{
			_factory = factory;
		}

		public void Add(Task task)
		{
			var query = @"INSERT INTO Tasks 
                          (Title, Description, StartDateTime, EndDateTime, Deadline, ProgressPercentage, IsCompleted, StudentId, CategoryId)
                          VALUES 
                          (@Title, @Description, @StartDateTime, @EndDateTime, @Deadline, @ProgressPercentage, @IsCompleted, @StudentId, @CategoryId)";

			using var connection = _factory.CreateConnection();
			using var command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@Title", task.Title);
			command.Parameters.AddWithValue("@Description", task.Description);
			command.Parameters.AddWithValue("@StartDateTime", task.StartTime);
			command.Parameters.AddWithValue("@EndDateTime", task.EndTime);
			command.Parameters.AddWithValue("@Deadline", (object?)task.Deadline ?? DBNull.Value);
			command.Parameters.AddWithValue("@ProgressPercentage", task.ProgressPercentage);
			command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
			command.Parameters.AddWithValue("@StudentId", task.StudentId);
			command.Parameters.AddWithValue("@CategoryId", (object?)task.CategoryId ?? DBNull.Value);

			connection.Open();
			command.ExecuteNonQuery();
		}

		public void Delete(int id)
		{
			var query = "DELETE FROM Tasks WHERE Id = @Id";

			using var connection = _factory.CreateConnection();
			using var command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@Id", id);

			connection.Open();
			command.ExecuteNonQuery();
		}

		public List<Task> GetAll()
		{
			var tasks = new List<Task>();

			using var connection = _factory.CreateConnection();
			using var command = new SqlCommand("SELECT * FROM Tasks", connection);

			connection.Open();
			using var reader = command.ExecuteReader();

			while (reader.Read())
			{
				tasks.Add(new Task
				{
					Id = (int)reader["Id"],
					Title = reader["Title"].ToString()!,
					Description = reader["Description"].ToString()!,
					StartTime = (DateTime)reader["StartDateTime"],
					EndTime = (DateTime)reader["EndDateTime"],
					Deadline = reader["Deadline"] as DateTime?,
					ProgressPercentage = (int)reader["ProgressPercentage"],
					IsCompleted = (bool)reader["IsCompleted"],
					StudentId = (int)reader["StudentId"],
					CategoryId = reader["CategoryId"] as int?
				});
			}

			return tasks;
		}

		public Task? GetById(int id)
		{
			using var connection = _factory.CreateConnection();
			using var command = new SqlCommand("SELECT * FROM Tasks WHERE Id = @Id", connection);

			command.Parameters.AddWithValue("@Id", id);

			connection.Open();
			using var reader = command.ExecuteReader();

			if (reader.Read())
			{
				return new Task
				{
					Id = (int)reader["Id"],
					Title = reader["Title"].ToString()!,
					Description = reader["Description"].ToString()!,
					StartTime = (DateTime)reader["StartDateTime"],
					EndTime = (DateTime)reader["EndDateTime"],
					Deadline = reader["Deadline"] as DateTime?,
					ProgressPercentage = (int)reader["ProgressPercentage"],
					IsCompleted = (bool)reader["IsCompleted"],
					StudentId = (int)reader["StudentId"],
					CategoryId = reader["CategoryId"] as int?
				};
			}

			return null;
		}

		public void Update(Task task)
		{
			var query = @"UPDATE Tasks SET 
                            Title=@Title, Description=@Description, StartDateTime=@StartDateTime, EndDateTime=@EndDateTime, 
                            Deadline=@Deadline, ProgressPercentage=@ProgressPercentage, IsCompleted=@IsCompleted, 
                            StudentId=@StudentId, CategoryId=@CategoryId 
                          WHERE Id=@Id";

			using var connection = _factory.CreateConnection();
			using var command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@Id", task.Id);
			command.Parameters.AddWithValue("@Title", task.Title);
			command.Parameters.AddWithValue("@Description", task.Description);
			command.Parameters.AddWithValue("@StartDateTime", task.StartTime);
			command.Parameters.AddWithValue("@EndDateTime", task.EndTime);
			command.Parameters.AddWithValue("@Deadline", (object?)task.Deadline ?? DBNull.Value);
			command.Parameters.AddWithValue("@ProgressPercentage", task.ProgressPercentage);
			command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
			command.Parameters.AddWithValue("@StudentId", task.StudentId);
			command.Parameters.AddWithValue("@CategoryId", (object?)task.CategoryId ?? DBNull.Value);

			connection.Open();
			command.ExecuteNonQuery();
		}
	}
}
