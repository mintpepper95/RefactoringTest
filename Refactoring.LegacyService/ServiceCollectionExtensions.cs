using Microsoft.Extensions.DependencyInjection;

namespace Refactoring.LegacyService;

public static class ServiceCollectionExtensions {
    // Call this method in the main project to configure for DI
    public static void AddCandidateService(this IServiceCollection services) {
        services
            .AddOptions<DatabaseOptions>()
            .BindConfiguration(DatabaseOptions.SectionName);
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddTransient<ITimeProvider, TimeProvider>();
        services.AddScoped<ICandidateFactory, CandidateFactory>();
        services.AddScoped<ICandidateCreditService, CandidateCreditServiceClient>();
    }
}
