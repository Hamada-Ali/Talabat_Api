using API.Extensions;
using API.Helpers;
using API.Middlewares;
using Core.Context;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                // connect to Redis Database ( connection string) with non more options
                var config = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("RedisConnection"), true);

                return ConnectionMultiplexer.Connect(config);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);
            //builder.Services.AddIdentityCore<AppUser>

            var app = builder.Build();

            await ApplySeeding.ApplyAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseMiddleware<ExceptionMiddleware>();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}