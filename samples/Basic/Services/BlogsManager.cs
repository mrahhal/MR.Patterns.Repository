using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Basic.Models;
using MR.Patterns.Repository;

namespace Basic.Services
{
	public class BlogsManager
	{
		private IRepository _repository;

		public BlogsManager(IRepository repository)
		{
			_repository = repository;
		}

		public Task<Blog> GetBlog(int id)
		{
			return _repository.Blogs.FindByIdAsync(id);
		}

		public Task<List<Blog>> GetBlogs()
		{
			return _repository.Blogs.ToListAsync();
		}

		public async Task<int> CreateNewBlog(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			var blog = new Blog
			{
				Name = name
			};
			await _repository.AddAsync(blog);
			return blog.Id;
		}

		public async Task<int> CreateNewPostInBlog(int blogId, string postTitle, string postContent)
		{
			if (postTitle == null)
			{
				throw new ArgumentNullException(nameof(postTitle));
			}

			var blog = await _repository.Blogs.FindByIdAsync(blogId);
			if (blog == null)
			{
				throw new ArgumentException($"Could not find the blog with blog id: {blogId}.");
			}

			var post = new Post
			{
				Blog = blog,
				Title = postTitle,
				Content = postContent
			};
			await _repository.AddAsync(post);
			return post.Id;
		}
	}
}
