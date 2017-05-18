using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MR.Patterns.Repository
{
	public class ReentranceTest : DatabaseTestHost
	{
		[Fact]
		public async Task RunInTransaction_IsReentrant()
		{
			using (var repo = CreateRepository())
			{
				await repo.AddAsync(new Blog());
			}

			using (var repo = CreateRepository())
			{
				repo.RunInTransaction((c, t) =>
				{
					repo.RunInTransaction((c2, t2) =>
					{
						var count = repo.Blogs.Count();
						count.Should().Be(1);
					});
				});
			}
		}

		[Fact]
		public async Task RunInTransactionAsync_IsReentrant()
		{
			using (var repo = CreateRepository())
			{
				await repo.AddAsync(new Blog());
			}

			using (var repo = CreateRepository())
			{
				await repo.RunInTransactionAsync((c, t) =>
				{
					return repo.RunInTransactionAsync(async (c2, t2) =>
					{
						var count = await repo.Blogs.CountAsync();
						count.Should().Be(1);
					});
				});
			}
		}
	}
}
