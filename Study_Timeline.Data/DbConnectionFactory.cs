using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class DbConnectionFactory
{
	private readonly string _connectionString;

	public DbConnectionFactory(IConfiguration config)
	{
		_connectionString = config.GetConnectionString("DefaultConnection");
	}

	public SqlConnection CreateConnection()
	{
		return new SqlConnection(_connectionString);
	}
}
