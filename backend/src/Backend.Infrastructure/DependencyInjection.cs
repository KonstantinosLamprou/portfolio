// Backend.Infrastructure/DependencyInjection.cs
using Backend.Domain.Interfaces;
using Backend.Infrastructure.Persistence;
using Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Backend.Application.Common.Interfaces;
using Backend.Infrastructure.Storage;
using Amazon.S3;
using Amazon.Runtime;

namespace Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddRepositories();

        return services;
    }

    public static IServiceCollection AddObjectStorageService(this IServiceCollection services)
    {
        var endpoint  = Environment.GetEnvironmentVariable("S3_ENDPOINT"); 
        var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY"); 
        var secretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");     

        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            new BasicAWSCredentials(accessKey, secretKey),
            new AmazonS3Config
            {
                ServiceURL = endpoint,
                ForcePathStyle = true,
                RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED,
                ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED
            }));

        services.AddScoped<IObjectStorageService, ObjectStorageService>();

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