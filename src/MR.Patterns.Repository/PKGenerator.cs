using System;
using System.Threading;

namespace MR.Patterns.Repository
{
	public class PKGenerator<TValue>
	{
		private long _current;

		public TValue Next()
			=> (TValue)Convert.ChangeType(Interlocked.Increment(ref _current), typeof(TValue));
	}
}
