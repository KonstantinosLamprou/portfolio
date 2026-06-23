using Backend.Application.UseCases.Content;
using Backend.Application.UseCases.Comments;
using Backend.Application.UseCases.Guestbook;
using Backend.Application.UseCases.Interactions;
using Backend.Application.UseCases.User;
using Microsoft.Extensions.DependencyInjection;

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