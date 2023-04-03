using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Riode.WebUI.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }


        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult WishList()
        {
            return View();
        }
    }
}
