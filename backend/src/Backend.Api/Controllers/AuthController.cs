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
        // 1. Gäste-Cookie von GitHub/Google auslesen
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticateResult.Succeeded) return Unauthorized();

        var claims = authenticateResult.Principal?.Identities.FirstOrDefault()?.Claims;
        if (claims == null) return BadRequest();

        var providerSubjectId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                   ?? claims.FirstOrDefault(c => c.Type == "urn:github:name")?.Value;
        var profilePictureUrl = claims.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value
                     ?? claims.FirstOrDefault(c => c.Type == "urn:github:avatar_url")?.Value
                     ?? claims.FirstOrDefault(c => c.Type == "picture")?.Value;

        if (providerSubjectId == null || email == null) return BadRequest("Unzureichende Daten vom Provider.");

        // 2. Command an deinen Handler schicken
        var cmd = new AddUserCommand(
            Provider: provider,
            ProviderSubjectId: providerSubjectId,
            Email: email,
            Name: name ?? email.Split('@')[0],
            ProfilePictureUrl: profilePictureUrl
        );

        // HIER IST DIE MAGIE: Wir fangen dein AddUserResult auf!
        var result = await _handler.Handle(cmd, HttpContext.RequestAborted);

        // 3. Eigene Claims mit DEINER Guid erstellen
        var internalClaims = new List<Claim>
        {
            // result.Id (bzw. wie auch immer die Property in deinem Record heißt) ist die Guid!
            new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()), 
            new Claim(ClaimTypes.Email, result.Email),
            new Claim(ClaimTypes.Name, result.Name),
            new Claim("Provider", result.Provider)
        };

        var internalIdentity = new ClaimsIdentity(internalClaims, CookieAuthenticationDefaults.AuthenticationScheme);

        // 4. Den User mit DEINEM Cookie neu anmelden
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(internalIdentity)
        );

        // 5. Zurück zum Vue-Frontend
        return Redirect("http://localhost:5173/auth/callback"); 
    }

    [HttpGet("session")]
    public IActionResult GetSession()
    {
        // Prüfen, ob das Cookie vorhanden und gültig ist
        if (User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
        }

        // Claims 
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        var name = User.FindFirst(ClaimTypes.Name)?.Value ?? User.Identity?.Name ?? string.Empty;
        var profilePictureUrl = User.FindFirst("ProfilePictureUrl")?.Value ?? string.Empty;
        // Den Provider auslesen
        var provider = User.FindFirst("Provider")?.Value ?? "Unknown";

        // (Falls die ID aus dem Claim keine gültige Guid ist, wird eine leere Guid erzeugt)
        Guid.TryParse(userIdString, out var userId);

        // 3. Dein DTO instanziieren
        var userDto = new UserDto(
            UserId: userId,
            Email: email,
            Name: name,
            Provider: provider,
            ProfilePictureUrl: profilePictureUrl
        );

        // TanStack Query es unter `data.user` direkt findet
        return Ok(new { user = userDto });
    }
}