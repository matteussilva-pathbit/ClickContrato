namespace ClickContrato.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        // Auth
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // Contratos
        services.AddSingleton<IContractTemplateRepository, InMemoryContractTemplateRepository>();
        services.AddSingleton<IContractDraftRepository, InMemoryContractDraftRepository>();

        return services;
    }
}


