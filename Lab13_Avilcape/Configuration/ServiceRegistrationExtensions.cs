using Infrastructure.Configuration;
using Microsoft.OpenApi.Models;

namespace API.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddInfrastructureServices(configuration);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title       = "Excel Reports API",
                Version     = "v1",
                Description = "Reportes Excel desde LINQExample."
            });
        });

        return services;
    }
}