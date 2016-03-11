using FluentAssertions;
using Xunit;

namespace MR.Patterns.Repository
{
	public class PKGeneratorTest : TestHost
	{
		[Fact]
		public void Int()
		{
			var generator = new PKGenerator<int>();

			generator.Next().Should().Be(1);
			generator.Next().Should().Be(2);
			generator.Next().Should().Be(3);
		}

		[Fact]
		public void Long()
		{
			var generator = new PKGenerator<long>();

			generator.Next().Should().Be(1L);
			generator.Next().Should().Be(2L);
			generator.Next().Should().Be(3L);
		}
	}
}
