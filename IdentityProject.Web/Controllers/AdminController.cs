using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityProject.Web.Models;
using IdentityProject.Web.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : base(userManager, null, roleManager)
        {
        }
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }
        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel model)
        {
            AppRole role = new AppRole();
            role.Name = model.Name;
            IdentityResult result = _roleManager.CreateAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                AddModelError(result);
            }
            return View(model);
        }
        public IActionResult RoleDeleted(string id)
        {
            AppRole role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;

            }
            return RedirectToAction("Roles");
        }
        public IActionResult RoleUpdated(string id)
        {
            AppRole role = _roleManager.FindByIdAsync(id).Result;
            if (role==null)
            {
                return RedirectToAction("Roles");
            }
            return View(role.Adapt<RoleViewModel>());
        }
        [HttpPost]
        public IActionResult RoleUpdated(RoleViewModel model)
        {
            AppRole role = _roleManager.FindByIdAsync(model.Id).Result;
            if (role!=null)
            {
                role.Name = model.Name;
                IdentityResult result = _roleManager.UpdateAsync(role).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelError(result);
                }
            }
            else
            {
                ModelState.AddModelError("","Role update failed.");
            }
            return View(model);
        }
        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }
        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;
            AppUser user = _userManager.FindByIdAsync(id).Result;

            ViewBag.username = user.UserName;
            var roles =  _roleManager.Roles;

            List<string> userRoles = _userManager.GetRolesAsync(user).Result as List<string>;

            List<RoleAssignViewModel> models = new List<RoleAssignViewModel>();

            foreach (var role in roles)
            {
                RoleAssignViewModel r = new RoleAssignViewModel {RoleId = role.Id, RoleName = role.Name};
                if (userRoles.Contains(role.Name))
                {
                    r.Exist = true;
                }
                else
                {
                    r.Exist = false;
                }
                models.Add(r);
                
            }
            return View(models);
        }
        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> list)
        {
            AppUser user = _userManager.FindByIdAsync(TempData["userId"].ToString()).Result;
            
            foreach (var role in list)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(user,role.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }
            return RedirectToAction("Users");
        }
        public IActionResult Claims()
        {
            return View(User.Claims.ToList());
        }
    }
}
