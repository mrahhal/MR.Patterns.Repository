using System;
using System.Threading.Tasks;
using Basic.Models;
using Basic.Services;
using Microsoft.Extensions.DependencyInjection;
using MR.Patterns.Repository;

namespace Basic
{
	public class Program
	{
		private IServiceProvider Provider { get; set; }

		public static void Main(string[] args)
		{
			new Program().Run();
		}

		private void Run()
		{
			Initialize();
		}

		private void Initialize()
		{
			Provider = ConfigureDI();
			using (var context = Provider.GetService<AppDbContext>())
			{
				TryInitializeDatabaseAsync(context).GetAwaiter().GetResult();
			}
			GoAsync().GetAwaiter().GetResult();
		}

		private async Task GoAsync()
		{
			using (var repository = Provider.GetService<IRepository>())
			{
				var posts = await repository.Posts.ToListAsync();
				Console.WriteLine($"There are {posts.Count} posts.");
				foreach (var post in posts)
				{
					Console.WriteLine($"Post: {post.Title}");
				}
			}
		}

		private async Task TryInitializeDatabaseAsync(AppDbContext context)
		{
			if (!await context.Blogs.AnyAsync())
			{
				context.Blogs.Add(new Blog()
				{
					Name = "My blog"
				});
				await context.SaveChangesAsync();
			}
			var blog = await context.Blogs.FirstAsync();
			if (!await context.Posts.AnyAsync())
			{
				blog.Posts.Add(new Post()
				{
					Title = "Some post title",
					Content = "Some post content"
				});
				await context.SaveChangesAsync();
			}
		}

		private IServiceProvider ConfigureDI()
		{
			var configuration = new DIConfiguration();
			configuration.ConfigureCommon();
			configuration.ConfigureDefaults();
			return configuration.Build();
		}
	}
}
