using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

		void Reload<T>(T entity)
			where T : class;

		Task ReloadAsync<T>(T entity)
			where T : class;

		void SetState<T>(T entity, EntityState state)
			where T : class;

		void SetPropertyModified<T>(T entity, string propertyName)
			where T : class;

		void SetPropertyModified<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
			where T : class;

		void Load<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
			where T : class
			where TProperty : class;

		Task LoadAsync<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
			where T : class
			where TProperty : class;

		void Load<T, TElement>(T entity, Expression<Func<T, ICollection<TElement>>> property)
			where T : class
			where TElement : class;

		Task LoadAsync<T, TElement>(T entity, Expression<Func<T, ICollection<TElement>>> property)
			where T : class
			where TElement : class;

		IQueryable<TElement> Query<T, TElement>(T entity, Expression<Func<T, ICollection<TElement>>> property)
			where T : class
			where TElement : class;
	}
}
