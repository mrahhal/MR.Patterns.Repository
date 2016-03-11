using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MR.Patterns.Repository
{
	public abstract class RepositoryCore<TContext> : IRepositoryCore
		 where TContext : DbContext
	{
		public RepositoryCore(TContext context)
		{
			Context = context;
		}

		public TContext Context { get; }

		public Database Database => Context.Database;

		public DbConnection Connection => Context.Database.Connection;

		public void Add<TEntity>(TEntity entity)
			where TEntity : class
		{
			Context.Set<TEntity>().Add(entity);
		}

		public void Update<TEntity>(TEntity entity)
			where TEntity : class
		{
			Context.Entry(entity).State = EntityState.Modified;
		}

		public void Remove<TEntity>(TEntity entity)
			where TEntity : class
		{
			Context.Set<TEntity>().Remove(entity);
		}

		public Task SaveChangesAsync() => Context.SaveChangesAsync();

		public Task RunInTransactionAsync(Func<Task> action)
			=> RunInTransactionAsync((_) => action());

		public async Task RunInTransactionAsync(Func<IDbTransaction, Task> action)
		{
			using (var transaction = Database.BeginTransaction())
			{
				try
				{
					await action(transaction.UnderlyingTransaction);

					await Context.SaveChangesAsync();
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public Task RunInTransactionAsync(Action action)
			=> RunInTransactionAsync((_) => action());

		public async Task RunInTransactionAsync(Action<IDbTransaction> action)
		{
			using (var transaction = Database.BeginTransaction())
			{
				try
				{
					action(transaction.UnderlyingTransaction);

					await Context.SaveChangesAsync();
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}
