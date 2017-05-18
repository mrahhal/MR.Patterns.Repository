using System;
using System.Data;
using Dapper;

namespace MR.Patterns.Repository
{
	public abstract class DatabaseTestHost : IDisposable
	{
		private static bool _sqlObjectInstalled;

		public DatabaseTestHost()
		{
			UnitTestDetector.SetIsInUnitTest(false);
			InitializeDatabase();
		}

		public virtual void Dispose()
		{
			DeleteAllData();
		}

		protected TestDbContext CreateContext() => new TestDbContext();

		protected TestRepository CreateRepository(TestDbContext context = null)
			=> new TestRepository(context ?? CreateContext());

		private void InitializeDatabase()
		{
			if (!_sqlObjectInstalled)
			{
				using (var context = CreateContext())
				{
					context.Database.Delete();
					context.Database.CreateIfNotExists();
					_sqlObjectInstalled = true;
				}
			}
		}

		private void DeleteAllData()
		{
			using (var context = CreateContext())
			{
				var commands = new[]
				{
					"DISABLE TRIGGER ALL ON ?",
					"ALTER TABLE ? NOCHECK CONSTRAINT ALL",
					"DELETE FROM ?",
					"ALTER TABLE ? CHECK CONSTRAINT ALL",
					"ENABLE TRIGGER ALL ON ?"
				};
				foreach (var command in commands)
				{
					context.Database.Connection.Execute(
						"sp_MSforeachtable",
						new { command1 = command },
						commandType: CommandType.StoredProcedure);
				}
			}
		}
	}
}
