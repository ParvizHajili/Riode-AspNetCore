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
        private readonly IConfiguration _configuration;
        public AccountController(SignInManager<RiodeUser> signInManager, UserManager<RiodeUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [Route("/signin.html")]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/signin.html")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginFormModel model)
        {
            if (ModelState.IsValid)
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
                {
                    return Redirect(callBackUrl);
                }

                return RedirectToAction("Index", "Shop");
            }

            end:
            return View(model);
        }


        [Route("/register.html")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/register.html")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new RiodeUser();

                user.Name = model.Name;
                user.SurName = model.SurName;
                user.UserName = model.Email;
                user.Email = model.Email;
                user.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    ViewBag.Message = "Qeydiyyat Tamamlandı";
                }
                #region EmailConfirm
                //if (result.Succeeded)
                //{
                //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //    string path = $"{Request.Scheme}://{Request.Host}/registration-confirm.html?token={token}";
                //    var emailResponse = _configuration.SendMail(user.Email, "Riode User Registration", $"Zəhmət olmasa <a href={path}>Link</a> vasitəsi ilə qeydiyyatı tamamlayın.");
                //    if (emailResponse)
                //    {
                //        ViewBag.Message = "Qeydiyyat Tamamlandı";
                //    }
                //    else
                //    {
                //        ViewBag.Message = "Yanlisliq oldu yeniden cehd edin";
                //    }

                //    return RedirectToAction(nameof(SignIn));
                //}
                #endregion


                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return RedirectToAction(nameof(SignIn));

            }

            return View(model);
        }

        [Route("/profile.html")]
        public IActionResult Profile()
        {
            return View();
        }

        [Route("/logout.html")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(SignIn));
        }

        public IActionResult WishList()
        {
            return View();
        }
    }
}
