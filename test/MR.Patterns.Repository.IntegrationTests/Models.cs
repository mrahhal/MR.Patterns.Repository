using System.Collections.Generic;

namespace MR.Patterns.Repository
{
	public class Blog : IEntity<int>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public List<Post> Posts = new List<Post>();
	}

	public class Post : IEntity<int>
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public Blog Blog { get; set; }
		public int BlogId { get; set; }
	}
}
