using Domain.Entities;
using System.Security.Claims;

namespace Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GerarTokenJWT(CWUsuarioMaster usuarioMaster);
        bool ValidarTokenJWT(string token);
        ClaimsPrincipal GetPrincipalFromToken(string token);
        string? GetClaimValue(string token, string claimType);
        IEnumerable<Claim>? GetTokenClaims(string token);
    }
}
