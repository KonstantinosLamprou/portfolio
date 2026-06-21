using Backend.Domain.Interfaces;
using Backend.Infrastructure;
using Backend.Infrastructure.Persistence.Repositories;
using Backend.Infrastructure.Persistence;
using Backend.Application.UseCases.User;  
using Backend.Application.UseCases.SaveContent;  
using Backend.Application.UseCases.Interactions;
using Backend.Application.UseCases.GetContent;
using Backend.Application.UseCases.Comments;
using Backend.Application.UseCases.Guestbook;
using Backend.Application.Options;
using Backend.Presentation.Workers; 
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System.Text.Json;
using System.Security.Claims;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using Backend.Api.Middlewares;


var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAuthentication(options =>
{
    // Standard-Scheme ist Cookie (wir vertrauen unserem eigenen Cookie)
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // Verhindert den Redirect bei 401 (Nicht eingeloggt)
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        
        // Verhindert den Redirect bei 403 (Fehlende Rechte)
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.SaveTokens = true; // Speichert Access Tokens 
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");

})
.AddGitHub(options =>
{
    options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"]!;
    options.Scope.Add("user:email"); 
    options.ClaimActions.MapJsonKey("urn:github:avatar_url", "avatar_url", "url");

});

builder.Services.Configure<AdminOptions>(
    builder.Configuration.GetSection("Admin")
);

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend API", Version = "v1" });
    options.MapType<JsonElement>(() => new OpenApiSchema { Type = Microsoft.OpenApi.JsonSchemaType.Object });
});

// Den Handler für die Dependency Injection registrieren
builder.Services.AddScoped<AddUserHandler>();

builder.Services.AddScoped<CreateContentHandler>();

builder.Services.AddScoped<AddLikeHandler>();

builder.Services.AddScoped<GetAllBlogsHandler>();
builder.Services.AddScoped<GetAllProjectsHandler>();
builder.Services.AddScoped<GetBlogDetailsHandler>();
builder.Services.AddScoped<GetProjectDetailsHandler>();
builder.Services.AddScoped<GetLatestBlogsHandler>();
builder.Services.AddScoped<UpdateViewsBlogHandler>();
builder.Services.AddScoped<UpdateViewsProjectHandler>();

builder.Services.AddScoped<CreateCommentHandler>();
builder.Services.AddScoped<GetCommentsHandler>();
builder.Services.AddScoped<UpdateCommentHandler>();
builder.Services.AddScoped<DeleteCommentHandler>();
builder.Services.AddScoped<GetCommentByIdHandler>();

builder.Services.AddScoped<CreateGuestbookEntryHandler>();
builder.Services.AddScoped<GetGuestbookEntriesHandler>();
builder.Services.AddScoped<GetGuestbookEntryHandler>();
builder.Services.AddScoped<UpdateGuestbookEntryHandler>();
builder.Services.AddScoped<DeleteGuestbookEntryHandler>();

builder.Services.AddScoped<UpdateCommentVoteHandler>(); 
builder.Services.AddScoped<DeleteCommentVoteHandler>();
builder.Services.AddScoped<GetCommentsVoteHandler>();
builder.Services.AddScoped<CreateCommentVoteHandler>();

builder.Services.AddScoped<GetStatisticsHandler>();
builder.Services.AddScoped<UpdateStatisticsHandler>();

builder.Services.AddScoped<IBlogInterface, EfBlogRepository>();
builder.Services.AddScoped<IProjectInterface, EfProjectRepository>();
builder.Services.AddScoped<IApplicationUserInterface, EfApplicationUserRepository>();
builder.Services.AddScoped<ILikeInterface, EfLikeRepository>();
builder.Services.AddScoped<ICommentInterface, EfCommentRepository>();
builder.Services.AddScoped<ICommentVoteInterface, EfCommentVoteRepository>();
builder.Services.AddScoped<IGuestbookEntry, EfGuestbookEntryRepository>();
builder.Services.AddScoped<IStatisticsInterface, EfStatisticsRepository>();
builder.Services.AddHostedService<ImageCleanupService>();


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Erlaube deinem Vue-Frontend den Zugriff
              .AllowAnyHeader()                     // Erlaube alle Header (z.B. Content-Type)
              .AllowAnyMethod()                     // Erlaube GET, POST, PUT, DELETE etc.
              .AllowCredentials();                  // EXTREM WICHTIG: Erlaubt das Senden von Cookies!
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseCors();

app.UseHttpsRedirection();
app.UseExceptionHandler(); 
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

