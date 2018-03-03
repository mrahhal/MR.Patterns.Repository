using System.Collections.Generic;

namespace MR.Patterns.Repository
{
	/// <summary>
	/// An <see cref="IEqualityComparer{T}"/> that compares the <see cref="IEntity{TKey}.Id"/> property of int entities.
	/// </summary>
	public class EntityIntEqualityComparer : IEqualityComparer<IEntity<int>>
	{
		public static readonly EntityIntEqualityComparer Instance = new EntityIntEqualityComparer();

		public bool Equals(IEntity<int> x, IEntity<int> y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(IEntity<int> obj)
		{
			return obj.Id.GetHashCode();
		}
	}

	/// <summary>
	/// An <see cref="IEqualityComparer{T}"/> that compares the <see cref="IEntity{TKey}.Id"/> property of string entities.
	/// </summary>
	public class EntityStringEqualityComparer : IEqualityComparer<IEntity<string>>
	{
		public static readonly EntityStringEqualityComparer Instance = new EntityStringEqualityComparer();

		public bool Equals(IEntity<string> x, IEntity<string> y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(IEntity<string> obj)
		{
			return obj.Id.GetHashCode();
		}
	}
}
