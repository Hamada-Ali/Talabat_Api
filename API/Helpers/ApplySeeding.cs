using Core.Context;
using Core.IdentityEntities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class ApplySeeding
    {
        public static async Task ApplyAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var context = services.GetRequiredService<StoreDbContext>();
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    await context.Database.MigrateAsync();
                    await AppIdentityContextSeed.SeedUserAsync(userManager);
                    await StoreContextSeed.SeedAsync(context, loggerFactory);

                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                    logger.LogError(ex.Message);
                }
            }
        }
    }
}
