using Backend.Domain.Interfaces;
using Backend.Infrastructure;
using Backend.Infrastructure.Persistence.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Backend.Application.Options;
using Backend.Application.UseCases.User;  
using Backend.Application.UseCases.SaveContent;  
using Backend.Application.UseCases.Interactions;
using Backend.Application.UseCases.GetContent;

using DotNetEnv;
using Microsoft.OpenApi;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Backend.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAuthentication(options =>
{
    // Standard-Scheme ist Cookie (wir vertrauen unserem eigenen Cookie)
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Optionaler Fallback
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

    // options.Events.OnCreatingTicket = async context =>
    // {
    //     var email = context.Identity?.FindFirst(ClaimTypes.Email)?.Value;

    //     if (!string.IsNullOrEmpty(email))
    //     {
    //         var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

    //         var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

    //         if (user != null && user.Role == Backend.Domain.Entities.UserRole.Admin)
    //         {
    //             context.Identity?.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
    //         }
    //     }
    // };
})
.AddGitHub(options =>
{
    options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"]!;
    options.Scope.Add("user:email"); 

    // GitHub gibt E-Mails oft nur über die API zurück, dieser Scope hilft dabei:
     options.Events.OnCreatingTicket = async context =>
     {
        var email = context.Identity?.FindFirst(ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(email))
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && user.Role == Backend.Domain.Entities.UserRole.Admin)
            {
                context.Identity?.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }
        }
    };
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
builder.Services.AddScoped<IBlogInterface, EfBlogRepository>();
builder.Services.AddScoped<IProjectInterface, EfProjectRepository>();
builder.Services.AddScoped<IApplicationUserInterface, EfApplicationUserRepository>();
builder.Services.AddScoped<ILikeInterface, EfLikeRepository>();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

