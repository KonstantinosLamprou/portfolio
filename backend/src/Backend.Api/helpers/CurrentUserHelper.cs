using System.Security.Claims;

namespace Backend.Api.Helpers;
public static class CurrentUserHelper
{
    public static Guid? GetCurrentUserIdFromClaims(ClaimsPrincipal user)
    {
        var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
            return null;

        return Guid.TryParse(userIdString, out var userId) ? userId : null;
    }
}