using InfrastructureLayer.Data;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IntegrationTests
{
	public class TestDatabaseFixture
	{
		private const string ConnectionString = @"Data Source=localhost;Initial Catalog=LibraryDatapac;Integrated Security=True;Trust Server Certificate=True";

		private static readonly object _lock = new();
		private static bool _databaseInitialized;

		public TestDatabaseFixture()
		{
			lock (_lock)
			{
				if (!_databaseInitialized)
				{
					using (var context = CreateContext())
					{
						context.Database.EnsureDeleted();
						context.Database.EnsureCreated();

						SeedData.InitializeAsync(context).Wait();
						context.SaveChanges();
					}

					_databaseInitialized = true;
				}
			}
		}

		public LibraryContext CreateContext()
			 => new LibraryContext(
				  new DbContextOptionsBuilder<LibraryContext>()
						.UseSqlServer(ConnectionString)
						.Options);

		public ILogger<T>? CreateLogger<T>()
		{
			var serviceProvider = new ServiceCollection()
			 .AddLogging(builder => builder.AddDebug()) // log to the debug output window
			 .BuildServiceProvider();

			var factory = serviceProvider.GetService<ILoggerFactory>();
			return factory?.CreateLogger<T>();
		}
	}
}
