using System.Collections.Generic;
using MR.Patterns.Repository;

namespace Basic.Models
{
	public class Blog : IEntity<int>
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public List<Post> Posts { get; set; } = new List<Post>();
	}
}
