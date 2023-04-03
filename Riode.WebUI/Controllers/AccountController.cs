using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.AppCode.Extensions;
using Riode.WebUI.Models.FormModels;
using Riode.WebUI.Models.Membership;

namespace Riode.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<RiodeUser> _signInManager;
        private readonly UserManager<RiodeUser> _userManager;
        public AccountController(SignInManager<RiodeUser> signInManager, UserManager<RiodeUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginFormModel model)
        {
            RiodeUser foundedUser = null;

            if (model.UserName.IsEmail())
            {
                foundedUser = await _userManager.FindByEmailAsync(model.UserName);
            }
            else
            {
                foundedUser = await _userManager.FindByNameAsync(model.UserName);
            }

            if (foundedUser == null)
            {
                ViewBag.Message = "İstifadəçi adı və ya şifrə yanlışdır.";
                goto end;
            }

            var result = await _signInManager.PasswordSignInAsync(foundedUser, model.Password, true, true);
            if (!result.Succeeded)
            {
                ViewBag.Message = "İstifadəçi adı və ya şifrə yanlışdır.";
                goto end;
            }

            var callBackUrl = Request.Query["ReturnUrl"];
            if (!string.IsNullOrWhiteSpace(callBackUrl))
                return Redirect(callBackUrl);
            else
                return RedirectToAction("Shop", "Index");

            end:
            return View(model);
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
