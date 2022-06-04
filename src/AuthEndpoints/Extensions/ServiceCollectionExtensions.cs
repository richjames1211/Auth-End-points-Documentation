﻿using AuthEndpoints.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace AuthEndpoints;

/// <summary>
/// Provides extensions to easily bootstrap authendpoints
/// </summary>
public static class ServiceCollectionExtensions
{
    internal static AuthEndpointsBuilder ConfigureServices<TUserKey, TUser>(IServiceCollection services)
        where TUserKey : IEquatable<TUserKey>
        where TUser : IdentityUser<TUserKey>
    {
        services.TryAddSingleton<IPostConfigureOptions<AuthEndpointsOptions>, OptionsConfigurator>();
        services.TryAddSingleton<IValidateOptions<AuthEndpointsOptions>, OptionsValidator>();

        // Add authendpoints core services
        services.TryAddScoped<IClaimsProvider<TUser>, DefaultClaimsProvider<TUserKey, TUser>>();
        services.TryAddScoped<IJwtFactory, DefaultJwtFactory>();
        services.TryAddScoped<IJwtValidator, DefaultJwtValidator>();
        services.TryAddScoped<IAuthenticator<TUser>, DefaultAuthenticator<TUser>>();

        services.TryAddSingleton<IEmailFactory, DefaultMessageFactory>();
        services.TryAddSingleton<IEmailSender, DefaultEmailSender>();

        services.TryAddScoped<IdentityErrorDescriber>();
        services.TryAddScoped<JwtSecurityTokenHandler>();

        var identityBuilder = services.AddIdentityCore<TUser>();

        return new AuthEndpointsBuilder(typeof(TUser), services, identityBuilder);
    }

    /// <summary>
    /// Adds the AuthEndpoints core services
    /// </summary>
    /// <typeparam name="TUserKey"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="services"></param>
    /// <returns>A <see cref="AuthEndpointsBuilder"/> for creating and configuring the AuthEndpoints system.</returns>
    public static AuthEndpointsBuilder AddAuthEndpoints<TUserKey, TUser>(this IServiceCollection services)
        where TUserKey : IEquatable<TUserKey>
        where TUser : IdentityUser<TUserKey>
    {
        services.AddOptions<AuthEndpointsOptions>();
        return services.AddAuthEndpoints<TUserKey, TUser>(o => { });
    }

    /// <summary>
    /// Adds and configures the AuthEndpoints system.
    /// </summary>
    /// <typeparam name="TUserKey">The type representing a User's primary key in the system.</typeparam>
    /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
    /// <param name="services">The services available in the application.</param>
    /// <param name="setup">An action to configure the <see cref="AuthEndpointsOptions"/>.</param>
    /// <returns>A <see cref="AuthEndpointsBuilder"/> for creating and configuring the AuthEndpoints system.</returns>
    public static AuthEndpointsBuilder AddAuthEndpoints<TUserKey, TUser>(this IServiceCollection services, Action<AuthEndpointsOptions> setup)
        where TUserKey : IEquatable<TUserKey>
        where TUser : IdentityUser<TUserKey>
    {
        if (setup != null)
        {
            services.AddOptions<AuthEndpointsOptions>()
                .Configure(setup);
        }

        return ConfigureServices<TUserKey, TUser>(services);
    }
}
