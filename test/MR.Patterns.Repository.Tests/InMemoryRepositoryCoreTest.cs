using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MR.Patterns.Repository
{
	public class InMemoryRepositoryCoreTest
	{
		private class Blog : IEntity<int>
		{
			public int Id { get; set; }
			public string Name { get; set; }

			public List<Post> Posts { get; set; }
		}

		private class Post : IEntity<long>
		{
			public long Id { get; set; }
			public string Title { get; set; }

			public Blog Blog { get; set; }
			public int BlogId { get; set; }
		}

		public class Some
		{
			public int Foo { get; set; }
		}

		private class InMemoryRepository : InMemoryRepositoryCore
		{
			public IQueryable<Blog> Blogs => For<Blog>();

			public IQueryable<Post> Posts => For<Post>();

			public IQueryable<Some> Somes => For<Some>();
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

		[Fact]
		public async Task Add_Some()
		{
			var repo = Create();
			var some = new Some()
			{
				Foo = 1
			};
			repo.Add(some);
			(await repo.Somes.Where(s => s.Foo == 1).FirstOrDefaultAsync()).Should().NotBeNull();
		}

		[Fact]
		public async Task Query()
		{
			var repo = Create();
			var blog = new Blog()
			{
				Posts = new List<Post> { new Post(), new Post() }
			};

			repo.Add(blog);

			(await repo.Query(blog, b => b.Posts).ToListAsync()).Should().HaveCount(2);
		}

		private InMemoryRepository Create() => new InMemoryRepository();
	}
}
