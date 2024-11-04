using InfrastructureLayer.Data;
using InfrastructureLayer.Email;
using InfrastructureLayer.Repositories;
using LibraryCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ReminderJob
{
   internal class Program
   {
		private static string LibraryEmail = "info@library.sk";
		private static string LibraryEmailSubjectTemplate = "CleanCodeLibrary: One day to return the book {0}";
		private static string LibraryEmailBodyTemplate = "Dear {0},{1} There is only one day remaining to return the book. Book is due {2}.{3}Best Regards{4}Clean Code Library Team";
		private static string NewLine = "\r\n";

		static async Task Main(string[] args)
      {
			var logger = Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

			// Create service collection
			var serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);

			var config = GetConfiguration();
			string? connectionString = config.GetConnectionString("DefaultConnection");
			serviceCollection.AddDbContext<LibraryContext>(options => options
				.UseSqlServer(connectionString)
			);

			// Build service provider
			var serviceProvider = serviceCollection.BuildServiceProvider();
			await SeedDatabase(serviceProvider);

			// Resolve service and use it
			var _repository = serviceProvider.GetService<ILibraryRepository>();
			if (_repository == null) { 
				// ToDO: Log
				Environment.Exit(1);
			}

			var _emailService = serviceProvider.GetService<IEmailSender>();
			if (_emailService == null)
			{
				// ToDO: Log
				Environment.Exit(2);
			}

			// ToDo: handle nulls, e.g. borrowing.Book, ...
			await foreach (var borrowing in _repository.GetBorrowingsDayBeforeExpirationAsync())
			{
				string subject = string.Format(LibraryEmailSubjectTemplate, borrowing.Book.Title);
				string body = string.Format(LibraryEmailBodyTemplate, borrowing.User.FullName, NewLine, borrowing.DateShouldReturn, NewLine, NewLine);
				bool emailQueued = await _emailService.SendEmailAsync(borrowing.User.Email, LibraryEmail, subject, body);
				if(emailQueued)
				{
					// ToDo: Continue by setting on Borrowing entity that notification email has been sent
				}
			}

			Environment.Exit(0);
		}

		private static void ConfigureServices(IServiceCollection services)
      {
         services.AddTransient<ILibraryRepository, LibraryRepository>();
         services.AddTransient<IEmailSender, FakeEmailSender>();
			services.AddLogging(config => { config.ClearProviders(); config.AddSerilog(); });
		}

		private static async Task SeedDatabase(ServiceProvider provider)
		{
			try
			{
				var context = provider.GetRequiredService<LibraryContext>();
				//          context.Database.Migrate();
				context.Database.EnsureCreated();
				await SeedData.InitializeAsync(context);
			}
			catch (Exception ex)
			{
				var logger = provider.GetRequiredService<ILogger<Program>>();
				logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
			}
		}

		private static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder()
							  .SetBasePath(Directory.GetCurrentDirectory())
							  .AddJsonFile("appsettings.json", optional: false);

			return builder.Build();
		}
   }
}
