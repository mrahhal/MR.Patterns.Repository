using System.Data.Entity;

namespace MR.Patterns.Repository
{
	public class TestDbContext : DbContext
	{
		public static string ConnectionString { get; set; }
			= "Server=(localdb)\\mssqllocaldb;Database=MR.Patterns.Repository.IntegrationTests;Trusted_Connection=True;MultipleActiveResultSets=true";

		public TestDbContext() : base(ConnectionString)
		{
		}

		public DbSet<Blog> Blogs { get; set; }

		public DbSet<Post> Posts { get; set; }
	}
}
