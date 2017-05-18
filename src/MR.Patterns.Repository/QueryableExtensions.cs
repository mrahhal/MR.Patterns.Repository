using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using QE = System.Data.Entity.QueryableExtensions;

namespace MR.Patterns.Repository
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> ById<T>(this IQueryable<T> @this, int id)
			where T : class, IEntity<int>
			=> @this.Where(e => e.Id == id);

		public static IQueryable<T> ById<T>(this IQueryable<T> @this, long id)
			where T : class, IEntity<long>
			=> @this.Where(e => e.Id == id);

		public static IQueryable<T> ById<T>(this IQueryable<T> @this, string id)
			where T : class, IEntity<string>
			=> @this.Where(e => e.Id == id);

		public static Task<T> FindByIdAsync<T>(this IQueryable<T> @this, int id)
			where T : class, IEntity<int>
			=> @this.ById(id).FirstOrDefaultAsync();

		public static Task<T> FindByIdAsync<T>(this IQueryable<T> @this, long id)
			where T : class, IEntity<long>
			=> @this.ById(id).FirstOrDefaultAsync();

		public static Task<T> FindByIdAsync<T>(this IQueryable<T> @this, string id)
			where T : class, IEntity<string>
			=> @this.ById(id).FirstOrDefaultAsync();

		//------------------------------------------------------------------------------
		// Shims over System.Data.Entity.QueryableExtensions to ease out testing
		//------------------------------------------------------------------------------

		private static bool IsInUnitTest => UnitTestDetector.IsInUnitTest;

		public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source, Expression<Func<T, TProperty>> path)
		{
			if (IsInUnitTest)
				return source;
			return QE.Include(source, path);
		}

		public static IQueryable<T> AsNoTracking<T>(this IQueryable<T> source)
			where T : class
		{
			if (IsInUnitTest)
				return source;
			return QE.AsNoTracking(source);
		}

		public static Task<int> CountAsync<T>(this IQueryable<T> source)
		{
			if (IsInUnitTest)
				return Task.FromResult(source.Count());
			return QE.CountAsync(source);
		}

		public static Task<T> FirstAsync<T>(this IQueryable<T> source)
		{
			if (IsInUnitTest)
				return Task.FromResult(source.First());
			return QE.FirstAsync(source);
		}

		public static Task<T> FirstAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
		{
			if (IsInUnitTest)
				return Task.FromResult(source.First(predicate));
			return QE.FirstAsync(source, predicate);
		}

		public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source)
		{
			if (IsInUnitTest)
				return Task.FromResult(source.FirstOrDefault());
			return QE.FirstOrDefaultAsync(source);
		}

		public static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
		{
			if (IsInUnitTest)
				return Task.FromResult(source.FirstOrDefault(predicate));
			return QE.FirstOrDefaultAsync(source, predicate);
		}

		public static Task<bool> AnyAsync<T>(this IQueryable<T> source)
		{
			if (IsInUnitTest)
				return Task.FromResult(source.Any());
			return QE.AnyAsync(source);
		}

		public static Task<bool> AnyAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
		{
			if (IsInUnitTest)
				return Task.FromResult(source.Any(predicate));
			return QE.AnyAsync(source, predicate);
		}

		public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> source)
		{
			if (IsInUnitTest)
				return Task.FromResult(source.ToArray());
			return QE.ToArrayAsync(source);
		}

		public static Task<List<T>> ToListAsync<T>(this IQueryable<T> source)
		{
			if (IsInUnitTest)
				return Task.FromResult(source.ToList());
			return QE.ToListAsync(source);
		}
	}
}
