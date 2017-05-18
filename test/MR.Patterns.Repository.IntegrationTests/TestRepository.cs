using System.Linq;

namespace MR.Patterns.Repository
{
	public class TestRepository : RepositoryCore<TestDbContext>
	{
		public TestRepository(TestDbContext context) : base(context)
		{
		}

		public IQueryable<Blog> Blogs => Context.Blogs;

		public IQueryable<Post> Posts => Context.Posts;
	}
}
