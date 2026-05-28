using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Backend.Application.UseCases.User;
using Backend.Domain.Contracts; 

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AddUserHandler _handler;

    public AuthController(AddUserHandler handler)
    {
        _handler = handler;
    }

    private static readonly Dictionary<string, string> ProviderMap =
    new(StringComparer.OrdinalIgnoreCase)
    {
        ["google"] = "Google",
        ["github"] = "GitHub"
    };

    private static bool TryMapProvider(string? raw, out string scheme)
    {
        scheme = string.Empty;

        if (string.IsNullOrWhiteSpace(raw))
            return false;

        if (ProviderMap.TryGetValue(raw, out var mapped) && !string.IsNullOrWhiteSpace(mapped))
            {
                // wenn true dann mapped in dem Fall "Google" oder "GitHub"
                scheme = mapped;
                return true;
            }

    return false;
    }

    // Endpunkt zum Starten des Logins (z.B. /api/auth/github/login)
    [HttpGet("{provider}/login")]
    public IActionResult Login([FromRoute] string provider)
    {
        // Provider-Mapping z.B. "google" -> "Google", "github" -> "GitHub"
        if (!TryMapProvider(provider, out var scheme))
                return BadRequest("Unknown provider.");

        var properties = new AuthenticationProperties
            {
                RedirectUri = $"/api/auth/{provider}/callback"
            };

        return Challenge(properties, scheme);
    }

    // Endpunkt, der von Google/GitHub aufgerufen wird, nachdem der User zugestimmt hat
    [HttpGet("{provider}/callback")]
    public async Task<IActionResult> Callback([FromRoute] string provider)
    {
        // Liest das temporaere Cookie aus, das von Google/GitHub gesetzt wurde
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded)
        {
            return Unauthorized();
        }

        var claims = authenticateResult.Principal?.Identities.FirstOrDefault()?.Claims;
        if (claims == null)
        {
            return BadRequest();
        }

        // Provider-spezifische Claim-Typen auslesen (z.B. NameIdentifier, Email, Name)
        var providerSubjectId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                   ?? claims.FirstOrDefault(c => c.Type == "urn:github:name")?.Value;

        if (providerSubjectId == null || email == null)
        {
            return BadRequest("Unzureichende Daten vom Provider.");
        }

        // im Callback nach dem Claim-Check:
        var cmd = new AddUserCommand(
            Provider: provider,
            ProviderSubjectId: providerSubjectId,
            Email: email,
            Name: name ?? email.Split('@')[0],
            ProfilePictureUrl: null
        );

        await _handler.Handle(cmd, HttpContext.RequestAborted);

        return Redirect("http://localhost:5173/api/auth/callback"); 
    }
}