namespace MR.Patterns.Repository
{
	public class TestHost
	{
		static TestHost()
		{
			UnitTestDetector.SetIsInUnitTest();
		}
	}
}
