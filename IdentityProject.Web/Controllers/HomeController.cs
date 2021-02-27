﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityProject.Web.Models;
using IdentityProject.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn(string returnUrl)
        {

            TempData["ReturnUrl"] = returnUrl;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {

                AppUser user = await _userManager.FindByEmailAsync(userLogin.Email);

                if (user != null)
                {
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "You tried too many incorrect entries, try later.");
                        return View(userLogin);
                    }

                    await _signInManager.SignOutAsync();
                    SignInResult result = await _signInManager.PasswordSignInAsync(user, userLogin.Password, userLogin.RememberMe, false);

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }


                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);


                        int fail = await _userManager.GetAccessFailedCountAsync(user);

                        if (fail == 5)
                        {
                            await _userManager.SetLockoutEndDateAsync(user,
                                  new DateTimeOffset(DateTime.Now.AddMinutes(30)));
                            ModelState.AddModelError("", "You tried too many incorrect entries, try later.");
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(LoginViewModel.Email), "Invalid email or password.");
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(LoginViewModel.Email), "Invalid user.");
                }
            }



            return View(userLogin);
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;

                IdentityResult result = await _userManager.CreateAsync(user, userViewModel.Password);
                if (result.Succeeded)
                {

                    return RedirectToAction("LogIn");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                }
            }


            return View(userViewModel);
        }
    }
}
