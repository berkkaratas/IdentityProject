using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityProject.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Web.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager):base(userManager,null,roleManager)
        {
            
        }

        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }
    }
}
