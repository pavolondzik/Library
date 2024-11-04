using Microsoft.Extensions.Logging;
using InfrastructureLayer;
using LibraryCore.Interfaces;
using InfrastructureLayer.Email;

namespace LibraryManagementApi.Configurations
{
   public static class ServiceConfigs
   {
      public static IServiceCollection AddServiceConfigs(this IServiceCollection services, ILogger logger, WebApplicationBuilder builder)
      {
         services.AddInfrastructureServices(builder.Configuration, logger);

         if (builder.Environment.IsDevelopment())
         {
            builder.Services.AddScoped<IEmailSender, FakeEmailSender>();

         }
         else
         {
            services.AddScoped<IEmailSender, SmtpEmailSender>();
         }

         logger.LogInformation("{Project} services registered", "MediatR and Email Sender");

         return services;
      }
   }
}
