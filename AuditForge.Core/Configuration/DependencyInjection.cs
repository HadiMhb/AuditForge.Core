using Microsoft.Extensions.DependencyInjection;
using AuditForge.Configuration;
using AuditForge.Application.Interfaces;
using AuditForge.Application.Services;

namespace AuditForge.Core.Configuration;

/// <summary>
/// Provides extension methods to register AuditForge services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds AuditForge services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Optional action to configure audit options.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAuditForge(this IServiceCollection services, Action<AuditOptions>? configure = null)
    {
        var options = new AuditOptions();
        configure?.Invoke(options);

        services.AddSingleton(options);
        services.AddScoped<IAuditService, AuditService>();

        return services;
    }
}
