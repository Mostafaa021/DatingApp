using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions 
    {
        public static string GetUsername (this ClaimsPrincipal principals) {
            
            return principals.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}