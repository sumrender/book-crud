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
                // Use SQL Server with retry logic and connection pooling
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString, 
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            // Enable retry on failure for transient errors
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                            
                            // Configure connection pooling and performance settings
                            sqlOptions.MaxBatchSize(100);
                            sqlOptions.CommandTimeout(30);
                        }));
            }
        }
    }
} 