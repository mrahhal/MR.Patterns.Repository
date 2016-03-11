using System;
using System.Reflection;

namespace MR.Patterns.Repository
{
	public static class ReflectionHelper
	{
		public static bool IsDefaultCountValue(PropertyInfo property, object obj)
		{
			if (property.PropertyType == typeof(int))
				return property.GetValue(obj).Equals((int)0);
			else if (property.PropertyType == typeof(long))
				return property.GetValue(obj).Equals((long)0);
			else
				throw new InvalidOperationException("The property's type is expected to be either an int or a long.");
		}

		public static PropertyInfo GetIdProperty(Type type) => type.GetProperty("Id");

		public static bool IsCountType(Type type)
			=>
			type == typeof(int) ||
			type == typeof(long);
	}
}
