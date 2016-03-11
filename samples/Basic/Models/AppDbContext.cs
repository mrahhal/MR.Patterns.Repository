using System.Data.Entity;

namespace Basic.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
		}

		public DbSet<Blog> Blogs { get; set; }

		public DbSet<Post> Posts { get; set; }
	}
}
