using InfrastructureLayer.Data;
using InfrastructureLayer.Repositories;
using LibraryCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfrastructureLayer
{
	public static class InfrastructureServiceExtensions
   {
      public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
         Microsoft.Extensions.Configuration.ConfigurationManager config, ILogger logger)
      {
         string? connectionString = config.GetConnectionString("DefaultConnection");
         services.AddDbContext<LibraryContext>(options => options
            .UseSqlServer(connectionString)
			//.LogTo(Serilog.Log.Information, LogLevel.Information)
			);

         services.AddScoped(typeof(ILibraryRepository), typeof(LibraryRepository));
			
			logger.LogInformation("{Project} services registered", "Infrastructure");


         return services;
      }
   }
}
