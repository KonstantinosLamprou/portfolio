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
    private readonly UpsertOAuthUserHandler _handler;

    public AuthController(UpsertOAuthUserHandler handler)
    {
        _handler = handler;
    }

    // Endpunkt zum Starten des Logins (z.B. /api/auth/github/login)
    [HttpGet("{provider}/login")]
    public IActionResult Login([FromRoute] string provider)
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = $"/api/auth/{provider}/callback"
        };

        // Leitet den User zu Google oder GitHub weiter
        return Challenge(properties, new[] { provider });
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

        // Wichtige Daten extrahieren
        var providerSubjectId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                   ?? claims.FirstOrDefault(c => c.Type == "urn:github:name")?.Value;

        if (providerSubjectId == null || email == null)
        {
            return BadRequest("Unzureichende Daten vom Provider.");
        }

        // im Callback nach dem Claim-Check:
        var cmd = new UpsertOAuthUserCommand(
            Provider: provider,
            ProviderSubjectId: providerSubjectId,
            Email: email,
            Name: name ?? email.Split('@')[0],
            ProfilePictureUrl: null
        );

        await _handler.Handle(cmd, HttpContext.RequestAborted);

        return Redirect("http://localhost:5173/auth/callback"); 
    }
}