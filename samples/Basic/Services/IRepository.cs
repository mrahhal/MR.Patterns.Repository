using System;
using System.Linq;
using Basic.Models;
using MR.Patterns.Repository;

namespace Basic.Services
{
	public interface IRepository : IRepositoryCore
	{
		IQueryable<Blog> Blogs { get; }

		IQueryable<Post> Posts { get; }
	}
}
