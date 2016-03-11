using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MR.Patterns.Repository.Internal;

namespace MR.Patterns.Repository
{
	public abstract class InMemoryRepositoryCore : IRepositoryCore
	{
		private Dictionary<Type, Table> _tables = new Dictionary<Type, Table>();

		public void Add<TEntity>(TEntity entity)
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

		public void Update<TEntity>(TEntity entity)
			where TEntity : class
		{
		}

		public void Remove<TEntity>(TEntity entity)
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

		public Task SaveChangesAsync() => Task.FromResult(0);

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

		public void Dispose()
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
				if (ReflectionHelper.IsCountType(_idPI.PropertyType))
				{
					_pkGenerator = Activator.CreateInstance(
						typeof(PKGenerator<>).MakeGenericType(_idPI.PropertyType));
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
