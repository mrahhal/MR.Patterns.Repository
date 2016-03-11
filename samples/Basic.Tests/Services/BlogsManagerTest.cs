using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using FluentAssertions;
using System;

namespace Basic.Services
{
	public class BlogsManagerTest : TestHost
	{
		[Fact]
		public async Task GetBlogs_Empty()
		{
			var provider = CreateProvider();
			var manager = provider.GetService<BlogsManager>();

			var blogs = await manager.GetBlogs();

			blogs.Should().BeEmpty();
		}

		[Fact]
		public void CreateNewBlog_ArgumentNullCheck()
		{
			var provider = CreateProvider();
			var manager = provider.GetService<BlogsManager>();

			Assert.Throws<ArgumentNullException>(() => manager.CreateNewBlog(null).GetAwaiter().GetResult());
		}

		[Fact]
		public async Task CreateNewBlog()
		{
			var provider = CreateProvider();
			var repo = provider.GetService<IRepository>();
			var manager = provider.GetService<BlogsManager>();

			var blogId = await manager.CreateNewBlog("Foo");

			blogId.Should().NotBe(0);
		}

		[Fact]
		public async Task CreateNewBlog_StoresBlog()
		{
			var provider = CreateProvider();
			var repo = provider.GetService<IRepository>();
			var manager = provider.GetService<BlogsManager>();

			var blogId = await manager.CreateNewBlog("Foo");

			(await manager.GetBlog(blogId)).Should().NotBeNull();
		}
	}
}
