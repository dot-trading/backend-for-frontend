using Microsoft.Extensions.DependencyInjection;

namespace Bff.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) => services;
}
