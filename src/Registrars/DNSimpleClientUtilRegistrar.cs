using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.DNSimple.Client.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.DNSimple.Client.Registrars;

/// <summary>
/// An async thread-safe singleton for the DNSimple client
/// </summary>
public static class DNSimpleClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IDNSimpleClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddDNSimpleClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton();
        services.TryAddSingleton<IDNSimpleClientUtil, DNSimpleClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IDNSimpleClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddDNSimpleClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton();
        services.TryAddScoped<IDNSimpleClientUtil, DNSimpleClientUtil>();

        return services;
    }
}
