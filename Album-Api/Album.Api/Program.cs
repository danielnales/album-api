using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Album.Api.Services;
using Album.Api.Models;
using Album.Api.Data;
using Microsoft.OpenApi.Models;

namespace Album.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddScoped<IGreetingService, GreetingService>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Album API", Version = "v1"});
            });

            builder.Services.AddDbContext<AlbumContext>(options =>
            {
                options.UseNpgsql("Host=cnsd-db-934870780723.czxid8ngsmgl.us-east-1.rds.amazonaws.com;Port=5432;Database=albumdatabase;Username=postgres;Password=postgres;");
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Album API v1");
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AlbumContext>();
                DBInitializer.Initialize(context);
            }

            app.MapControllers();

            app.Run();
        }
    }
}
