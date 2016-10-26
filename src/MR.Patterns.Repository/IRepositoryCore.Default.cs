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

		public virtual void Add<TEntity>(TEntity entity)
			where TEntity : class
		{
			Context.Set<TEntity>().Add(entity);
		}

		public virtual void Update<TEntity>(TEntity entity)
			where TEntity : class
		{
			Context.Entry(entity).State = EntityState.Modified;
		}

		public virtual void Remove<TEntity>(TEntity entity)
			where TEntity : class
		{
			Context.Set<TEntity>().Remove(entity);
		}

		public virtual Task SaveChangesAsync() => Context.SaveChangesAsync();

		public void RunInTransaction(Action<DbConnection, DbTransaction> action, IsolationLevel? isolationLevel = null)
		{
			RunInTransaction((connection, transaction) =>
			{
				action(connection, transaction);
				return true;
			}, isolationLevel);
		}

		public Task RunInTransactionAsync(Func<DbConnection, DbTransaction, Task> func, IsolationLevel? isolationLevel = null)
		{
			return RunInTransactionAsync(async (connection, transaction) =>
			{
				await func(connection, transaction);
				return true;
			}, isolationLevel);
		}

		public T RunInTransaction<T>(Func<DbConnection, DbTransaction, T> func, IsolationLevel? isolationLevel = null)
		{
			T result;
			using (var contextTransaction = CreateTransaction(isolationLevel))
			{
				try
				{
					result = func(Database.Connection, contextTransaction.UnderlyingTransaction);
					contextTransaction.Commit();
				}
				catch
				{
					contextTransaction.Rollback();
					throw;
				}
			}
			return result;
		}

		public async Task<T> RunInTransactionAsync<T>(Func<DbConnection, DbTransaction, Task<T>> func, IsolationLevel? isolationLevel = null)
		{
			T result;
			using (var contextTransaction = CreateTransaction(isolationLevel))
			{
				try
				{
					result = await func(Database.Connection, contextTransaction.UnderlyingTransaction);
					contextTransaction.Commit();
				}
				catch
				{
					contextTransaction.Rollback();
					throw;
				}
			}
			return result;
		}

		private DbContextTransaction CreateTransaction(IsolationLevel? isolationLevel)
		{
			return
				isolationLevel == null ?
				Database.BeginTransaction() :
				Database.BeginTransaction(isolationLevel.Value);
		}

		public virtual void Dispose()
		{
			Context.Dispose();
		}

		// Obsolete

		[Obsolete("Use the other overloads with manually calling SaveChangesAsync when necessary.")]
		public virtual Task RunInTransactionAsync(Func<Task> action)
			=> RunInTransactionAsync((_) => action());

		[Obsolete("Use the other overloads with manually calling SaveChangesAsync when necessary.")]
		public virtual Task RunInTransactionAsync(Func<IDbTransaction, Task> action)
		{
			return RunInTransactionAsync(async (_, transaction) =>
			{
				await action(transaction);
				await SaveChangesAsync();
			});
		}

		[Obsolete("Use the other overloads with manually calling SaveChangesAsync when necessary.")]
		public virtual Task RunInTransactionAsync(Action action)
			=> RunInTransactionAsync((_) => action());

		[Obsolete("Use the other overloads with manually calling SaveChangesAsync when necessary.")]
		public virtual Task RunInTransactionAsync(Action<IDbTransaction> action)
		{
			return RunInTransactionAsync((_, transaction) =>
			{
				action(transaction);
				return SaveChangesAsync();
			});
		}
	}
}
