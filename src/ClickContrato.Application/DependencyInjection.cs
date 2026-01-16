using Microsoft.Extensions.DependencyInjection;

namespace ClickContrato.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<ContractService>();
        return services;
    }
}


