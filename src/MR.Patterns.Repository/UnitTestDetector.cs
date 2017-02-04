using System;
using System.Linq;
using System.Reflection;

namespace MR.Patterns.Repository
{
	public static class UnitTestDetector
	{
		private static readonly string[] TestAssemblies = new[]
		{
			"xunit.core",
			"nunit.framework"
		};

		private static bool _set = IsTestAssemblyLoaded();

		public static bool IsInUnitTest => _set;

		public static void SetIsInUnitTest(bool value = true) => _set = value;

		private static bool IsTestAssemblyLoaded()
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (TestAssemblies
						.Any(testAssemblyName => assembly.FullName.ToLowerInvariant().StartsWith(testAssemblyName)))
				{
					return true;
				}
			}
			return false;
		}
	}
}
