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


var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAuthentication(options =>
{
    // Standard-Scheme ist Cookie (wir vertrauen unserem eigenen Cookie)
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Optionaler Fallback
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.SaveTokens = true; // Speichert Access Tokens (falls du später Google-APIs aufrufen willst)
})
.AddGitHub(options =>
{
    options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"]!;
    // GitHub gibt E-Mails oft nur über die API zurück, dieser Scope hilft dabei:
    options.Scope.Add("user:email"); 
});

builder.Services.Configure<AdminOptions>(
    builder.Configuration.GetSection("Admin")
);

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

