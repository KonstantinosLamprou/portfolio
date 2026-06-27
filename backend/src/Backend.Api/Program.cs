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


var builder = WebApplication.CreateBuilder(args);

// Logger Lokal
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) 
    .Enrich.FromLogContext()
    .WriteTo.Console() 
    .WriteTo.Seq("http://seq:80") 
    .CreateLogger();

builder.Host.UseSerilog();

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

builder.Services.AddMemoryCache();

builder.Services.AddInfrastructureServices(builder.Configuration);

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
        policy.WithOrigins("http://localhost:5173") 
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

app.UseHttpsRedirection();
app.UseExceptionHandler(); 
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

