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
        protected RoleManager<AppRole> _roleManager;
        protected AppUser CurrentUser =>  _userManager.FindByNameAsync(User.Identity.Name).Result;

        public BaseController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager=null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
