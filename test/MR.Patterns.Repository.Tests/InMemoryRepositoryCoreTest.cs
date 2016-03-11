using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MR.Patterns.Repository
{
	public class InMemoryRepositoryCoreTest : TestHost
	{
		private class Blog : IEntity<int>
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

		private class Post : IEntity<long>
		{
			public long Id { get; set; }
			public string Title { get; set; }
		}

		private class InMemoryRepository : InMemoryRepositoryCore
		{
			public IQueryable<Blog> Blogs => For<Blog>();

			public IQueryable<Post> Posts => For<Post>();
		}

		[Fact]
		public void Empty()
		{
			var repo = Create();

			repo.Blogs.ToList().Should().BeEmpty();
		}

		[Fact]
		public async Task Add()
		{
			var repo = Create();
			var blog = new Blog();

			repo.Add(blog);

			blog.Id.Should().NotBe(0);

			(await repo.Blogs.FindByIdAsync(blog.Id)).Should().NotBeNull();
		}

		[Fact]
		public void Update()
		{
			var repo = Create();
			var blog = new Blog();

			repo.Add(blog);
			repo.Update(blog);

			blog.Id.Should().NotBe(0);
		}

		[Fact]
		public async Task Remove()
		{
			var repo = Create();
			var blog = new Blog();
			repo.Add(blog);

			repo.Remove(blog);

			(await repo.Blogs.FindByIdAsync(blog.Id)).Should().BeNull();
		}

		private InMemoryRepository Create() => new InMemoryRepository();
	}
}
