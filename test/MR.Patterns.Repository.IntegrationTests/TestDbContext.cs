using System;
using System.Data.Entity;
using System.Data.SqlClient;

namespace MR.Patterns.Repository
{
	public class TestDbContext : DbContext
	{
		public static string ConnectionString { get; set; } = ConnectionUtil.GetConnectionString();

		public TestDbContext() : base(ConnectionString)
		{
		}

		public DbSet<Blog> Blogs { get; set; }

		public DbSet<Post> Posts { get; set; }
	}

	public static class ConnectionUtil
	{
		private const string ConnectionStringTemplateVariable = "Jobs_SqlServer_ConnectionStringTemplate";

		private const string DefaultDatabaseName = @"MR.Patterns.Repository.IntegrationTests";

		private const string DefaultConnectionStringTemplate = "Server=(localdb)\\mssqllocaldb;Database={0};Trusted_Connection=True;";

		public static string GetConnectionString()
		{
			return string.Format(GetConnectionStringTemplate(), DefaultDatabaseName);
		}

		private static string GetConnectionStringTemplate()
		{
			return
				Environment.GetEnvironmentVariable(ConnectionStringTemplateVariable) ??
				DefaultConnectionStringTemplate;
		}

		public static SqlConnection CreateConnection(string connectionString = null)
		{
			connectionString = connectionString ?? GetConnectionString();
			var connection = new SqlConnection(connectionString);
			connection.Open();
			return connection;
		}
	}
}
