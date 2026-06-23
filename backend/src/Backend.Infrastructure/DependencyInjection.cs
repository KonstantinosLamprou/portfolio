// Backend.Infrastructure/DependencyInjection.cs
using Backend.Domain.Interfaces;
using Backend.Infrastructure.Persistence;
using Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // DbContext mit PostgreSQL konfigurieren
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Repositories registrieren
        services.AddRepositories();

        return services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBlogInterface, BlogRepository>();
        services.AddScoped<IProjectInterface, ProjectRepository>();
        services.AddScoped<IApplicationUserInterface, ApplicationUserRepository>();
        services.AddScoped<ILikeInterface, LikeRepository>();
        services.AddScoped<ICommentInterface, CommentRepository>();
        services.AddScoped<ICommentVoteInterface, CommentVoteRepository>();
        services.AddScoped<IGuestbookEntry, GuestbookEntryRepository>();
        services.AddScoped<IStatisticsInterface, StatisticsRepository>();

        return services;
    }
}