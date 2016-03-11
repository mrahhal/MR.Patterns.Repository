using System.Collections.Generic;
using System.Threading.Tasks;

namespace MR.Patterns.Repository
{
	public static class RepositoryExtensions
	{
		public static Task AddAsync<TEntity>(this IRepositoryCore @this, TEntity entity)
			where TEntity : class
		{
			@this.Add(entity);
			return @this.SaveChangesAsync();
		}

		public static void AddRange<TEntity>(this IRepositoryCore @this, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			foreach (var entity in entities)
			{
				@this.Add(entity);
			}
		}

		public static Task AddRangeAsync<TEntity>(this IRepositoryCore @this, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			@this.AddRange(entities);
			return @this.SaveChangesAsync();
		}

		public static Task UpdateAsync<TEntity>(this IRepositoryCore @this, TEntity entity)
			where TEntity : class
		{
			@this.Update(entity);
			return @this.SaveChangesAsync();
		}

		public static void UpdateRange<TEntity>(this IRepositoryCore @this, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			foreach (var entity in entities)
			{
				@this.Update(entity);
			}
		}

		public static Task UpdateRangeAsync<TEntity>(this IRepositoryCore @this, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			@this.UpdateRange(entities);
			return @this.SaveChangesAsync();
		}

		public static Task RemoveAsync<TEntity>(this IRepositoryCore @this, TEntity entity)
			where TEntity : class
		{
			@this.Remove(entity);
			return @this.SaveChangesAsync();
		}

		public static void RemoveRange<TEntity>(this IRepositoryCore @this, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			foreach (var entity in entities)
			{
				@this.Remove(entity);
			}
		}

		public static Task RemoveRangeAsync<TEntity>(this IRepositoryCore @this, IEnumerable<TEntity> entities)
			where TEntity : class
		{
			@this.RemoveRange(entities);
			return @this.SaveChangesAsync();
		}
	}
}
