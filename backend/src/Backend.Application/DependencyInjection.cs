using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Decorators;
using FluentValidation;

namespace Backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly);

        // 1. Scrutor sucht automatisch ALLE Klassen, die IQueryHandler ODER ICommandHandler implementieren
        services.Scan(selector => selector
            .FromAssemblies(assembly)
            .AddClasses(filter => filter.AssignableToAny(
                typeof(IQueryHandler<,>),
                typeof(ICommandHandler<,>)
            ))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // 2. Query Decorators (NUR für Read-Operationen)
        // Die Reihenfolge ist wichtig: Von innen (Cache) nach außen (Logging)
        services.Decorate(typeof(IQueryHandler<,>), typeof(CachingDecorator<,>));
        services.Decorate(typeof(IQueryHandler<,>), typeof(QueryPerformanceLoggingDecorator<,>));

        // 3. Command Decorators (NUR für Write-Operationen)
        // Handlers ohne ICacheInvalidatorCommand (wie AddUserHandler) werden hier einfach durchgereicht
        // 1. Schicht (Ganz innen): Daten validieren (Bricht sofort ab bei Fehlern)
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator<,>));

        // 2. Schicht: Cache invalidieren (Läuft nur, wenn Daten valide waren)
        services.Decorate(typeof(ICommandHandler<,>), typeof(CacheInvalidationDecorator<,>));

        // 3. Schicht (Ganz außen): Performance messen
        services.Decorate(typeof(ICommandHandler<,>), typeof(CommandPerformanceLoggingDecorator<,>));
        
        // Weitere Application-Services könnten hier registriert werden
        // z.B. Validatoren, Mapper, etc.

        return services;
    }
}