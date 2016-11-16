using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MR.Patterns.Repository
{
	public interface IRepositoryCore : IDisposable
	{
		void Add<TEntity>(TEntity entity) where TEntity : class;

		void Update<TEntity>(TEntity entity) where TEntity : class;

		void Remove<TEntity>(TEntity entity) where TEntity : class;

		Task SaveChangesAsync();

		void RunInTransaction(
			Action<DbConnection, DbTransaction> action, IsolationLevel? isolationLevel = null);

		T RunInTransaction<T>(
			Func<DbConnection, DbTransaction, T> func, IsolationLevel? isolationLevel = null);

		Task RunInTransactionAsync(
			Func<DbConnection, DbTransaction, Task> func, IsolationLevel? isolationLevel = null);

		Task<T> RunInTransactionAsync<T>(
			Func<DbConnection, DbTransaction, Task<T>> func, IsolationLevel? isolationLevel = null);
	}
}
