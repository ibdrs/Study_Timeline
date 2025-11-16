using Microsoft.Data.SqlClient;
using Study_Timeline.Logic.Interfaces;
using DomainTask = Study_Timeline.Logic.Domain.Task;

namespace Study_Timeline.Data.Repositories
{
	public class TaskRepository : ITaskRepository
	{
		private readonly DbConnectionFactory _factory;
		public TaskRepository(DbConnectionFactory factory)
		{
			_factory = factory;
		}

		public async Task AddAsync(DomainTask task)
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

			await connection.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var query = "DELETE FROM Tasks WHERE Id = @Id";

			using var connection = _factory.CreateConnection();
			using var command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@Id", id);

			await connection.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<List<DomainTask>> GetAllAsync()
		{
			var tasks = new List<DomainTask>();

			using var connection = _factory.CreateConnection();
			using var command = new SqlCommand("SELECT * FROM Tasks", connection);
			await connection.OpenAsync();
			using var reader = await command.ExecuteReaderAsync();

			while (await reader.ReadAsync())
			{
				tasks.Add(new DomainTask
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

		public async Task<DomainTask?> GetByIdAsync(int id)
		{
			using var connection = _factory.CreateConnection();
			using var command = new SqlCommand("SELECT * FROM Tasks WHERE Id = @Id", connection);
			command.Parameters.AddWithValue("@Id", id);
			await connection.OpenAsync();

			using var reader = await command.ExecuteReaderAsync();
			if (await reader.ReadAsync())
			{
				return new DomainTask
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

		public async Task UpdateAsync(DomainTask task)
		{
			using var connection = _factory.CreateConnection();

			var query = @"UPDATE Tasks SET 
                            Title=@Title, Description=@Description, StartDateTime=@StartDateTime, EndDateTime=@EndDateTime, 
                            Deadline=@Deadline, ProgressPercentage=@ProgressPercentage, IsCompleted=@IsCompleted, 
                            StudentId=@StudentId, CategoryId=@CategoryId 
                          WHERE Id=@Id";

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

			await connection.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}
