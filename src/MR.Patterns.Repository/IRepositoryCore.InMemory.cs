using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MR.Patterns.Repository.Internal;

namespace MR.Patterns.Repository
{
	public abstract class InMemoryRepositoryCore : IRepositoryCore
	{
		private Dictionary<Type, Table> _tables = new Dictionary<Type, Table>();

		public virtual void Add<TEntity>(TEntity entity)
			where TEntity : class
		{
			var table = EnsureTable<TEntity>();
			EnsurePK(table, entity);
			var entities = table.Entities as List<TEntity>;
			if (!entities.Contains(entity))
			{
				entities.Add(entity);
			}
		}

		public virtual void Update<TEntity>(TEntity entity)
			where TEntity : class
		{
		}

		public virtual void Remove<TEntity>(TEntity entity)
			where TEntity : class
		{
			var table = EnsureTable<TEntity>();
			var entities = table.Entities as List<TEntity>;
			if (entities.Contains(entity))
			{
				entities.Remove(entity);
			}
		}

		protected IQueryable<TEntity> For<TEntity>()
			where TEntity : class
		{
			var table = EnsureTable<TEntity>();
			var entities = table.Entities as List<TEntity>;
			return entities.AsQueryable();
		}

		public virtual Task SaveChangesAsync() => Task.FromResult(0);

		//------------------------------------------------------------------------------

		private void EnsurePK(Table table, object entity)
		{
			table.EnsurePK(entity);
		}

		private Table FindTable<TEntity>()
			where TEntity : class
		{
			var entityType = typeof(TEntity);
			if (_tables.ContainsKey(entityType))
			{
				return _tables[entityType];
			}
			return null;
		}

		private Table EnsureTable<TEntity>()
			where TEntity : class
		{
			var entityType = typeof(TEntity);
			var table = FindTable<TEntity>();
			if (table == null)
			{
				table = _tables[entityType] =
					Activator.CreateInstance(typeof(Table<>).MakeGenericType(entityType)) as Table;
			}
			return table;
		}

		public void RunInTransaction(
			Action<DbConnection, DbTransaction> action, IsolationLevel? isolationLevel)
		{
			RunInTransaction((_1, _2) =>
			{
				action(_1, _2);
				return true;
			}, isolationLevel);
		}

		public T RunInTransaction<T>(
			Func<DbConnection, DbTransaction, T> func, IsolationLevel? isolationLevel)
		{
			return func(null, null);
		}

		public Task RunInTransactionAsync(
			Func<DbConnection, DbTransaction, Task> func, IsolationLevel? isolationLevel)
		{
			return RunInTransactionAsync((_1, _2) =>
			{
				func(_1, _2).GetAwaiter().GetResult();
				return Task.FromResult(true);
			}, isolationLevel);
		}

		public Task<T> RunInTransactionAsync<T>(
			Func<DbConnection, DbTransaction, Task<T>> func, IsolationLevel? isolationLevel)
		{
			return func(null, null);
		}
		public void Load<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
			where T : class
			where TProperty : class
		{
		}

		public Task LoadAsync<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
			where T : class
			where TProperty : class
		{
			return Task.FromResult(0);
		}

		public void Load<T, TElement>(T entity, Expression<Func<T, ICollection<TElement>>> property)
			where T : class
			where TElement : class
		{
		}

		public Task LoadAsync<T, TElement>(T entity, Expression<Func<T, ICollection<TElement>>> property)
			where T : class
			where TElement : class
		{
			return Task.FromResult(0);
		}

		public IQueryable<TElement> Query<T, TElement>(T entity, Expression<Func<T, ICollection<TElement>>> expression)
			where T : class
			where TElement : class
		{
			var pi = (expression.Body as MemberExpression).Member as PropertyInfo;
			var result = (ICollection<TElement>)pi.GetMethod.Invoke(entity, new object[0]);
			return result.AsQueryable();
		}

		public virtual void Dispose()
		{
		}

		private class Table
		{
			private Type _entityType;
			private PropertyInfo _idPI;
			private object _pkGenerator;

			public object Entities { get; }

			public Table(Type entityType)
			{
				_entityType = entityType;
				_idPI = ReflectionHelper.GetIdProperty(entityType);
				if (_idPI != null)
				{
					if (ReflectionHelper.IsCountType(_idPI.PropertyType))
					{
						_pkGenerator = Activator.CreateInstance(
							typeof(PKGenerator<>).MakeGenericType(_idPI.PropertyType));
					}
				}
				Entities = Activator.CreateInstance(typeof(List<>).MakeGenericType(_entityType));
			}

			public object NextKey()
			{
				if (_pkGenerator == null)
				{
					throw new InvalidOperationException("The PK's type is not int or long, so no need to generate a PK ourselves.");
				}
				return typeof(PKGenerator<>)
					.MakeGenericType(_idPI.PropertyType)
					.GetMethod("Next")
					.Invoke(_pkGenerator, new object[0]);
			}

			public void EnsurePK(object entity)
			{
				if (_pkGenerator != null && ReflectionHelper.IsDefaultCountValue(_idPI, entity))
				{
					_idPI.SetValue(entity, NextKey());
				}
			}
		}

		private class Table<TEntity> : Table
			where TEntity : class
		{
			public Table()
				: base(typeof(TEntity))
			{
			}
		}
	}
}
