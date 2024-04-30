using Business.Abstract;
using ETicaretUygulamasi.WebUI.EmailServices;
using ETicaretUygulamasi.WebUI.Extensions;
using ETicaretUygulamasi.WebUI.Identity;
using ETicaretUygulamasi.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaretUygulamasi.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]//tum post metodlarında get requestte serverdan gonderılen token ıle posttan servere giden aynı mı onu kontrol edıyor.
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;

        public AccountController(ICartService cartService,UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _cartService = cartService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl=null)
        {

            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var user = await _userManager.FindByNameAsync(model.Username);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı ile daha önce hesap oluşturulamıstır.");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen mail hesabınıza gelen mail ile hesabınızı onaylayın.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");
            }

            ModelState.AddModelError("", "Girilen kullanıcı adı ile parola yanlış.");

            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //generate token
                var tokenCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = tokenCode
                });

                //email
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı onaylayınız.", $"Email hesabınızı onaylamak için lütfen lınke <a href='https://localhost:44302{url}'>tıklayınız</a>");

                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen hata oldu tekrar deneyiniz");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData.Put("message", new AlertMessage()
            {
                Title = "Login",
                Message = "Your Account has been closed safely.",
                AlertType = "warning"
            });
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Token Info",
                    Message = "Invalid Token",
                    AlertType = "danger"
                });
                
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    //cart objesini oluştur
                    _cartService.InitializeCart(user.Id);
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Info",
                        Message = "Your Account has been approved.",
                        AlertType = "success"
                    });
                    
                    return View();
                }                
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Opps..",
                Message = "Your Account couldn' been approved.",
                AlertType = "warning"
            });
            
            
            return View();            
        }

       

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return View();                
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return View();
            }

            var tokenCode = await _userManager.GeneratePasswordResetTokenAsync(user);

            //generate token
            
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = tokenCode
            });

            Console.WriteLine(url);

            //email
            await _emailSender.SendEmailAsync(email, "Reset Password.", $"Parolanızı yenılemek için lütfen lınke <a href='https://localhost:44302{url}'>tıklayınız</a>");            

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Home", "Index");                
            }

            var model = new ResetPasswordModel()
            {
                Token = token
            };

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                RedirectToAction("Home", "Index");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Account", "Login");
            }

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
