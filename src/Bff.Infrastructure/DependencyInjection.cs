using Bff.Domain.Abstractions;
using Bff.Infrastructure.Options;
using Bff.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bff.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PersistenceOptions>(configuration.GetSection("Persistence"));
        services.Configure<ThirdPartyOptions>(configuration.GetSection("ThirdParty"));

        services.AddHttpClient<IThirdPartyService, ThirdPartyService>();
        services.AddHttpClient<IPersistenceService, PersistenceService>();

        return services;
    }
}
