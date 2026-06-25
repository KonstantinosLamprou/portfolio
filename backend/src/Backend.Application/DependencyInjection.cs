using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Decorators;
using Backend.Application.UseCases.Content;
using Backend.Application.UseCases.Comments;
using Backend.Application.UseCases.Guestbook;
using Backend.Application.UseCases.Interactions;
using Backend.Application.UseCases.User;

namespace Backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Handler registrieren
        services.AddHandlers();

        // Weitere Application-Services könnten hier registriert werden
        // z.B. Validatoren, Mapper, etc.

        return services;
    }
    // public static IServiceCollection AddApplication(this IServiceCollection services)
    // {
    //     var assembly = Assembly.GetExecutingAssembly();

    //     // 1. Scrutor sucht automatisch ALLE Klassen, die IQueryHandler implementieren, 
    //     // und registriert sie im Container.
    //     services.Scan(selector => selector
    //         .FromAssemblies(assembly)
    //         .AddClasses(filter => filter.AssignableTo(typeof(IQueryHandler<,>)))
    //         .AsImplementedInterfaces()
    //         .WithScopedLifetime());

    //     // 2. Die Matroschka-Puppe: Wir stülpen den PerformanceLogger über alle Handler!
    //     services.Decorate(typeof(IQueryHandler<,>), typeof(PerformanceLoggingDecorator<,>));

    // 2. Innere Hülle: Caching (Wird nur ausgeführt, wenn der Logger den Befehl durchreicht)
            // services.Decorate(typeof(IQueryHandler<,>), typeof(CachingDecorator<,>));

            // // 3. Äußere Hülle: Performance Logger (Stoppt die Zeit vom ERSTEN Klick an)
            // services.Decorate(typeof(IQueryHandler<,>), typeof(PerformanceLoggingDecorator<,>));

    //     return services;
    // }
    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        // User Handler
        services.AddScoped<AddUserHandler>();

        // Content Handler
        services.AddScoped<CreateContentHandler>();

        // Like Handler
        services.AddScoped<AddLikeHandler>();

        // Blog Handler
        services.AddScoped<GetAllBlogsHandler>();
        services.AddScoped<GetBlogDetailsHandler>();
        services.AddScoped<GetLatestBlogsHandler>();
        services.AddScoped<UpdateViewsBlogHandler>();

        // Project Handler
        services.AddScoped<GetAllProjectsHandler>();
        services.AddScoped<GetProjectDetailsHandler>();
        services.AddScoped<UpdateViewsProjectHandler>();

        // Comment Handler
        services.AddScoped<CreateCommentHandler>();
        services.AddScoped<GetCommentsHandler>();
        services.AddScoped<UpdateCommentHandler>();
        services.AddScoped<DeleteCommentHandler>();
        services.AddScoped<GetCommentByIdHandler>();

        // Guestbook Handler
        services.AddScoped<CreateGuestbookEntryHandler>();
        services.AddScoped<GetGuestbookEntriesHandler>();
        services.AddScoped<GetGuestbookEntryHandler>();
        services.AddScoped<UpdateGuestbookEntryHandler>();
        services.AddScoped<DeleteGuestbookEntryHandler>();

        // Comment Vote Handler
        services.AddScoped<UpdateCommentVoteHandler>();
        services.AddScoped<DeleteCommentVoteHandler>();
        services.AddScoped<GetCommentsVoteHandler>();
        services.AddScoped<CreateCommentVoteHandler>();

        // Statistics Handler
        services.AddScoped<GetStatisticsHandler>();
        services.AddScoped<UpdateStatisticsHandler>();

        return services;
    }
}