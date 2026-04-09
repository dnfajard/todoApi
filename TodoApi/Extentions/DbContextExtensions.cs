using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

namespace TodoApi.Extensions
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            return services;
        }
    }
}
