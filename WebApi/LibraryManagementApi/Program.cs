using LibraryManagementApi.Configurations;
using Serilog;
using Serilog.Extensions.Logging;

namespace LibraryManagementApi
{
	public class Program
   {
      public async static Task Main(string[] args)
      {
         var builder = WebApplication.CreateBuilder(args);

         var logger = Log.Logger = new LoggerConfiguration()
         .Enrich.FromLogContext()
         .WriteTo.Console()
         .CreateLogger();

         logger.Information("Starting web host");

         builder.AddLoggerConfigs();

         var appLogger = new SerilogLoggerFactory(logger).CreateLogger<Program>();
         builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
         builder.Services.AddServiceConfigs(appLogger, builder);

         builder.Services.AddControllers();
         builder.Services.AddEndpointsApiExplorer();
         builder.Services.AddSwaggerGen();

         var app = builder.Build();
			await app.UseAppMiddleware();

         app.Run();
      }
   }
}
