using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContexts;
using System.Security.Claims;

namespace Riode.WebUI.AppCode.Providers
{
    public class AppClaimProvider : IClaimsTransformation
    {
        private readonly RiodeDbContext _context;
        public AppClaimProvider(RiodeDbContext context)
        {
            _context = context;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if(principal.Identity.IsAuthenticated && principal.Identity is ClaimsIdentity currentIdentity)
            {
                var userId = Convert.ToInt32(currentIdentity.Claims.FirstOrDefault(c=>c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if(user != null)
                {
                    currentIdentity.AddClaim(new Claim("name", user.Name));
                    currentIdentity.AddClaim(new Claim("surname", user.SurName));
                }
            }
            return principal;
        }
    }
}
