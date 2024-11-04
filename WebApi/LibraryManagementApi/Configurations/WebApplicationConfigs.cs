using InfrastructureLayer.Data;
using LibraryManagementApi;

namespace LibraryManagementApi.Configurations
{
   public static class WebApplicationConfigs
   {
		public static async Task<IApplicationBuilder> UseAppMiddleware(this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			else
			{
				app.UseHsts();
			}

			app.UseAuthorization();
			app.MapControllers();

			await SeedDatabase(app);

			return app;
		}

		static async Task SeedDatabase(WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;

			try
			{
				var context = services.GetRequiredService<LibraryContext>();
				//          context.Database.Migrate();
				context.Database.EnsureCreated();
				await SeedData.InitializeAsync(context);
			}
			catch (Exception ex)
			{
				var logger = services.GetRequiredService<ILogger<Program>>();
				logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
			}
		}
	}
}
