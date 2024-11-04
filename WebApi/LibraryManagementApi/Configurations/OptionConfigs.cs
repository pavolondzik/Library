using InfrastructureLayer.Email;

namespace LibraryManagementApi.Configurations
{
   public static class OptionConfigs
   {
      public static IServiceCollection AddOptionConfigs(this IServiceCollection services, IConfiguration configuration, Microsoft.Extensions.Logging.ILogger logger, WebApplicationBuilder builder)
      {
         services.Configure<MailServerConfiguration>(configuration.GetSection("MailServer"));

         logger.LogInformation("{Project} were configured", "Options");

         return services;
      }
   }
}
