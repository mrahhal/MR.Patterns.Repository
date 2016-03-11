using System;
using Microsoft.Extensions.DependencyInjection;
using MR.Patterns.Repository;

namespace Basic
{
	public class TestHost
	{
		static TestHost()
		{
			UnitTestDetector.SetIsInUnitTest();
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
