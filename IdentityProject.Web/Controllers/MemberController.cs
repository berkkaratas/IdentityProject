using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityProject.Web.Models;
using IdentityProject.Web.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Web.Controllers
{
    public class MemberController : BaseController
    {
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager):base(userManager,signInManager){}
        [Authorize]
        public IActionResult Index()
        {

            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;

            UserViewModel model = user.Adapt<UserViewModel>();

            return View(model);
        }

        public IActionResult ProfileEdit()
        {
            AppUser user = CurrentUser;

            UserViewModel model = user.Adapt<UserViewModel>();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProfileEdit(UserViewModel model, IFormFile userPicture)
        {
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                AppUser user = CurrentUser;


                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", fileName);

                    await using var stream = new FileStream(path, FileMode.Create);
                    await userPicture.CopyToAsync(stream);
                    user.Picture = "/UserPicture/" + fileName;
                }
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    ViewBag.success = "true";
                    await _userManager.UpdateSecurityStampAsync(user);

                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);
                    ViewBag.success = "true";
                }
                else
                {
                    AddModelError(result);
                }



            }

            return View(model);
        }


        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PasswordChange(PasswordChangedViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = CurrentUser;

                if (user != null)
                {
                    bool exist = _userManager.CheckPasswordAsync(user, model.PasswordCurrent).Result;
                    if (exist)
                    {
                        IdentityResult result = _userManager
                            .ChangePasswordAsync(user, model.PasswordCurrent, model.PasswordNew).Result;
                        if (result.Succeeded)
                        {
                            _userManager.UpdateSecurityStampAsync(user);

                            _signInManager.SignOutAsync();
                            _signInManager.PasswordSignInAsync(user, model.PasswordNew, true, false);
                            ViewBag.success = "true";
                        }
                        else
                        {
                            AddModelError(result);
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect.");
                    }
                }

            }


            return View(model);
        }

        public void LogOut()
        {
            _signInManager.SignOutAsync();
        }

    }
}
