using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityProject.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Web.CustomValidation
{
    public class CustomPasswordValidator:IPasswordValidator<AppUser>
    {
        public  Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError(){Code = "PasswordContainsUserName",Description = "Password cannot contain username." });
            }
            if (password.ToLower().Contains("1234") && password.ToLower().Contains("5678"))
            {
                errors.Add(new IdentityError(){Code = "PasswordContains12345678",Description = "Password cannot contain consecutive numbers." });
            }
            if (password.ToLower().Contains(user.Email.ToLower()))
            {
                errors.Add(new IdentityError() { Code = "PasswordContainsEmail", Description = "Password cannot contain email." });
            }

            if (errors.Count==0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
