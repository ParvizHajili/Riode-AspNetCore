using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Membership;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly RiodeDbContext _context;
        public UsersController(RiodeDbContext context)
        {
            _context = context;
        }

        [Authorize("admin.users.index")]
        public async Task<IActionResult> Index()
        {
            var data = await _context.Users.ToListAsync();

            return View(data);
        }

        [Authorize("admin.users.details")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = await (from r in _context.Roles
                                   join ur in _context.UserRoles
                                   on new { RoleId = r.Id, UserId = user.Id } equals new { ur.RoleId, ur.UserId }
                                   into lJoin
                                   from lj in lJoin.DefaultIfEmpty()
                                   select Tuple.Create(r.Id, r.Name, lj != null)).ToListAsync();


            ViewBag.Principals = (from p in Program.principals
                                  join uc in _context.UserClaims on new { ClaimValue = "1", ClaimType = p, UserId = user.Id }
                                  equals new { uc.ClaimValue, uc.ClaimType, uc.UserId }
                                  into lJoin
                                  from lj in lJoin.DefaultIfEmpty()
                                  select Tuple.Create(p, lj != null)).ToList();

            return View(user);
        }

        [HttpPost]
        [Route("/user-set-role")]
        [Authorize("admin.users.setrole")]
        public async Task<IActionResult> SetRole(int userId, int roleId, bool selected)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
            if (user == null && role == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı Sorğu"
                });
            }

            if (selected)
            {
                if (await _context.UserRoles.AnyAsync(x => x.UserId == userId && x.RoleId == roleId))
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Name} {user.SurName} ' adlı istifadəçi '{role.Name}' adlı roldadır!"
                    });
                }
                else
                {
                    _context.UserRoles.Add(new RiodeUserRole
                    {
                        UserId = userId,
                        RoleId = roleId
                    });
                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Name} {user.SurName} ' adlı istifadəçi '{role.Name}' rola əlavə edildi!"
                    });
                }
            }
            else
            {
                var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);
                if (userRole == null)
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Name} {user.SurName} ' adlı istifadəçi '{role.Name}' adlı rolda deyil!"
                    });
                }
                else
                {
                    _context.UserRoles.Remove(userRole);

                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Name} {user.SurName} ' adlı istifadəçi '{role.Name}' roldan silindi!"
                    });
                }

            }

            return Json(null);
        }
    }
}
