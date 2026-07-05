using Backend.Infrastructure;
using Backend.Application.Options;
using Backend.Application; 
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
using Serilog;
using Backend.Application.Common.Interfaces;
using Backend.Infrastructure.Storage;
using Minio; 


var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    // Die .env wird nur lokal geladen (in Produktion übernimmt K8s das)
    Env.Load();
}
builder.Configuration.AddEnvironmentVariables();

var frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:5173";

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) 
    .Enrich.FromLogContext()
    .WriteTo.Console() 
    .WriteTo.Seq("http://seq-service:80") 
    .CreateLogger();

builder.Host.UseSerilog();

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

    options.Events.OnRemoteFailure = context =>
        {
            // 1. Die Exception abfangen und unterdrücken
            context.HandleResponse(); 

            // 2. Optional: Loggen, dass der Nutzer abgebrochen hat
            // (context.Failure enthält Details zur Exception)

            // 3. Den Nutzer zurück zum Frontend leiten
            // Passe die URL an die Route deines Vue-Frontends an
            context.Response.Redirect($"{frontendUrl}/auth/callback?error=cancelled"); 
            
            return Task.CompletedTask;
        };

})
.AddGitHub(options =>
{
    options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"]!;
    options.Scope.Add("user:email"); 
    options.ClaimActions.MapJsonKey("urn:github:avatar_url", "avatar_url", "url");
    options.Events.OnRemoteFailure = context =>
        {
            context.HandleResponse();
            context.Response.Redirect($"{frontendUrl}/auth/callback?error=cancelled");
            return Task.CompletedTask;
        };
});

builder.Services.Configure<AdminOptions>(
    builder.Configuration.GetSection("Admin")
);

builder.Services.AddMemoryCache();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddScoped<IMinIOService, MinIOStorageService>();

var minioEndpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT"); 
var minioAccessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY"); 
var minioSecretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY"); 

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(minioEndpoint)
    .WithCredentials(minioAccessKey, minioSecretKey)
    .WithSSL(false) 
    .Build());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend API", Version = "v1" });
    options.MapType<JsonElement>(() => new OpenApiSchema { Type = Microsoft.OpenApi.JsonSchemaType.Object });
});

// Handler 
builder.Services.AddApplicationServices();

builder.Services.AddHostedService<ImageCleanupService>();


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(frontendUrl) 
              .AllowAnyHeader()                     
              .AllowAnyMethod()                     
              .AllowCredentials();                 
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseCors();

// app.UseHttpsRedirection();

app.UseExceptionHandler(); 
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Datenbank-Migration beim Start der Anwendung

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    Log.Information("Starte Datenbank-Migration...");
    dbContext.Database.Migrate();
    Log.Information("Datenbank-Migration abgeschlossen.");
}

app.Run();

