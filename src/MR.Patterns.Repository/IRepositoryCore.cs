using System;
using System.Data;
using System.Threading.Tasks;

namespace MR.Patterns.Repository
{
	public interface IRepositoryCore : IDisposable
	{
		void Add<TEntity>(TEntity entity) where TEntity : class;

		void Update<TEntity>(TEntity entity) where TEntity : class;

		void Remove<TEntity>(TEntity entity) where TEntity : class;

		Task SaveChangesAsync();

		Task RunInTransactionAsync(Func<Task> action);

		Task RunInTransactionAsync(Func<IDbTransaction, Task> action);

		Task RunInTransactionAsync(Action action);

		Task RunInTransactionAsync(Action<IDbTransaction> action);
	}
}
