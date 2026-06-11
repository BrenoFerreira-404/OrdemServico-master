using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.Services.Auth;

/// <summary>
/// Extensões para extrair claims do JWT armazenado.
/// </summary>
public static class JwtClaimsHelper
{
    public static (Guid? UsuarioId, string? UsuarioNome) ObterUsuarioAtual(string? accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            return (null, null);

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);

            if (token.ValidTo < DateTime.UtcNow)
                return (null, null);

            var usuarioId = token.Claims.FirstOrDefault(c => c.Type == "usuario_id")?.Value;
            var nome = token.Claims.FirstOrDefault(c => c.Type == "name")?.Value ??
                       token.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;

            return (
                usuarioId is not null && Guid.TryParse(usuarioId, out var uid) ? uid : null,
                nome
            );
        }
        catch
        {
            return (null, null);
        }
    }
}