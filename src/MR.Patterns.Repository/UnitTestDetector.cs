namespace MR.Patterns.Repository
{
	public static class UnitTestDetector
	{
		private static bool _set;

		public static bool IsInUnitTest => _set;

		public static void SetIsInUnitTest() => _set = true;
	}
}
