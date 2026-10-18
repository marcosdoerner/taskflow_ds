using System.Security.Claims;

namespace TaskFlow.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int ObterUsuarioId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (claim is null || !int.TryParse(claim, out var id))
            throw new UnauthorizedAccessException(
                "Token não contém identificador de usuário válido."
            );
        return id;
    }
}
