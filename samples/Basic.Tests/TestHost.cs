using System;

namespace Basic
{
	public class TestHost
	{
		static TestHost()
		{
		}

		public IServiceProvider CreateProvider()
		{
			var configuration = new DIConfiguration();
			configuration.ConfigureCommon();
			configuration.ConfigureForTests();
			return configuration.Build();
		}
	}
}
