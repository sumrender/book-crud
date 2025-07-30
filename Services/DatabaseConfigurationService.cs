using Microsoft.EntityFrameworkCore;
using BooksCrudApi.Data;

namespace BooksCrudApi.Services
{
    public static class DatabaseConfigurationService
    {
        public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var useInMemoryDatabase = configuration.GetValue<bool>("DatabaseSettings:UseInMemoryDatabase");
            var databaseName = configuration.GetValue<string>("DatabaseSettings:DatabaseName") ?? "BooksCrudApi.Database";

            if (useInMemoryDatabase)
            {
                // Use In-Memory Database
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName));
            }
            else
            {
                // Use SQL Server
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString));
            }
        }
    }
} 