using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Serilog;

namespace RealTimeTaskManager.Data
{
    public class DesignTimeDBContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            Log.Information("DesignTimeDBContextFactory.CreateDbContext() called with args: {Args}", string.Join(", ", args));

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Use environment variable first, then fall back to default for design-time
            var envConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
            var defaultConnectionString = "Host=localhost;Port=5432;Database=RealTimeTaskManager;Username=postgres;Password=postgres";
            
            Log.Information("DesignTimeDBContextFactory - Environment variable: {EnvConnString}", 
                string.IsNullOrEmpty(envConnectionString) ? "NOT SET" : "SET (length: " + envConnectionString.Length + ")");
            
            var connectionString = envConnectionString ?? defaultConnectionString;
            
            Log.Information("DesignTimeDBContextFactory - Using connection string from: {Source}", 
                envConnectionString != null ? "Environment Variable" : "Default fallback");
            Log.Information("DesignTimeDBContextFactory - Connection string: Host={Host}, Database={Database}", 
                ExtractHost(connectionString), ExtractDatabase(connectionString));
            
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        private static string ExtractHost(string connectionString)
        {
            try
            {
                var parts = connectionString.Split(';');
                var hostPart = parts.FirstOrDefault(p => p.Trim().StartsWith("Host=", StringComparison.OrdinalIgnoreCase));
                return hostPart?.Split('=')[1] ?? "Unknown";
            }
            catch
            {
                return "Parse Error";
            }
        }

        private static string ExtractDatabase(string connectionString)
        {
            try
            {
                var parts = connectionString.Split(';');
                var dbPart = parts.FirstOrDefault(p => p.Trim().StartsWith("Database=", StringComparison.OrdinalIgnoreCase));
                return dbPart?.Split('=')[1] ?? "Unknown";
            }
            catch
            {
                return "Parse Error";
            }
        }
    }
}
