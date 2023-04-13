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
            if (principal.Identity.IsAuthenticated && principal.Identity is ClaimsIdentity currentIdentity)
            {
                var userId = Convert.ToInt32(currentIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    currentIdentity.AddClaim(new Claim("name", user.Name));
                    currentIdentity.AddClaim(new Claim("surname", user.SurName));
                }

                #region Reload Roles
                var role = currentIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
                while (role != null)
                {
                    currentIdentity.RemoveClaim(role);
                    role = currentIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
                }

                var currentRoles = (from ur in _context.UserRoles
                                    join r in _context.Roles on ur.RoleId equals r.Id
                                    where ur.UserId == userId
                                    select r.Name).ToArray();

                foreach (var item in currentRoles)
                {
                    currentIdentity.AddClaim(new Claim(ClaimTypes.Role, item));
                }
                #endregion

                #region Reload Claims for current user
                var currentClaims = currentIdentity.Claims.Where(c => Program.principals.Contains(c.Type))
                    .ToArray();

                foreach (var claim in currentClaims)
                {
                    currentIdentity.RemoveClaim(claim);
                }

                var currentPolicies = await (from uc in _context.UserClaims
                                             where uc.UserId == userId && uc.ClaimValue == "1"
                                             select uc.ClaimType)
                 .Union(from rc in _context.RoleClaims
                        join ur in _context.UserRoles on rc.RoleId equals ur.RoleId
                        where ur.UserId == userId && rc.ClaimValue == "1"
                        select rc.ClaimType).ToListAsync();

                foreach (var policy in currentPolicies)
                {
                    currentIdentity.AddClaim(new Claim(policy, "1"));
                }


                #endregion
            }
            return principal;
        }
    }
}
