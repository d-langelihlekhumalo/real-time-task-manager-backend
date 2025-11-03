namespace RealTimeTaskManager.Configuration
{
    public class CorsConfiguration
    {
        public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
        public bool AllowCredentials { get; set; } = true;
    }

    public class HealthChecksConfiguration
    {
        public bool Enabled { get; set; } = true;
        public bool DetailedErrors { get; set; } = false;
    }

    public class SwaggerConfiguration
    {
        public bool Enabled { get; set; } = true;
    }
}