using System.Linq;
using Basic.Models;
using MR.Patterns.Repository;

namespace Basic.Services
{
	public class InMemoryRepository : InMemoryRepositoryCore, IRepository
	{
		public IQueryable<Blog> Blogs => For<Blog>();

		public IQueryable<Post> Posts => For<Post>();
	}
}
