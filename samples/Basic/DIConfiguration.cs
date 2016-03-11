using System;
using System.Data.Entity;
using Basic.Models;
using Basic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Basic
{
	public class DIConfiguration
	{
		private IServiceCollection _services = new ServiceCollection();

		public IServiceCollection Services => _services;

		// Services that don't need to be mocked or changed between normal runs and tests goes here.
		public void ConfigureCommon()
		{
			_services.AddTransient<IMath, DefaultMath>();
			_services.AddTransient<BlogsManager>();
		}

		public void ConfigureDefaults()
		{
			// Typically, the DbContext should be scope registered in a web environment.
			// But this does it for a simple console app.
			_services.AddTransient(_ =>
				new AppDbContext("Server=(localdb)\\mssqllocaldb;Database=MR.Patterns.Repository-Basic;Trusted_Connection=True;MultipleActiveResultSets=true"));
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AppDbContext>());

			// Repository asks for DbContext so we should also do transient here.
			_services.AddTransient<IRepository, Repository>();
		}

		public void ConfigureForTests()
		{
			// We can't do scoped here, so singleton will
			// suffice in tests since we're always recreating the provider.
			_services.AddSingleton<IRepository, InMemoryRepository>();
		}

		public IServiceProvider Build() => _services.BuildServiceProvider();
	}
}
