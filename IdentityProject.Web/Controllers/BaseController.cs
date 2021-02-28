using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityProject.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Web.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppUser> _userManager;
        protected SignInManager<AppUser> _signInManager;
        protected AppUser CurrentUser =>  _userManager.FindByNameAsync(User.Identity.Name).Result;

        public BaseController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void AddModelError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("",error.Description);
            }
        }

    }
}
