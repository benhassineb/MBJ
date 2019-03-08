using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
namespace Common.WebApi
{
    /// <summary>
    ///     Implémente un ensemble d'extensions à <see cref="IPrincipal"/> pour récupérer des claims spécifiques.
    /// </summary>
    public static class PrincipalExtensions
    {
        public static string GetPreferredUserName(this ClaimsPrincipal principal)
        {
            return GetClaim(principal, ClaimTypes.PreferredUserName);
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return GetClaim(principal, ClaimTypes.Email);
        }

        private static string GetClaim(ClaimsPrincipal principal, string claimName)
        {
            return principal.Claims.FirstOrDefault(c => string.Equals(c.Type, claimName, StringComparison.OrdinalIgnoreCase))?.Value;
        }

    }
}
