using System.Linq;
using Basic.Models;
using MR.Patterns.Repository;

namespace Basic.Services
{
	public class Repository : RepositoryCore<AppDbContext>, IRepository
	{
		public Repository(AppDbContext context)
			: base(context)
		{
		}

		public IQueryable<Blog> Blogs => Context.Blogs;

		public IQueryable<Post> Posts => Context.Posts;
	}
}
