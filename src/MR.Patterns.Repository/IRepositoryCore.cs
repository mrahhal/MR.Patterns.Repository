using System;
using System.Threading.Tasks;

namespace MR.Patterns.Repository
{
	public interface IRepositoryCore : IDisposable
	{
		void Add<TEntity>(TEntity entity) where TEntity : class;

		void Update<TEntity>(TEntity entity) where TEntity : class;

		void Remove<TEntity>(TEntity entity) where TEntity : class;

		Task SaveChangesAsync();
	}
}
