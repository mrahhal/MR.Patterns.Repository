using MR.Patterns.Repository;

namespace Basic.Models
{
	public class Post : IEntity<int>
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public int BlogId { get; set; }
		public Blog Blog { get; set; }
	}
}
