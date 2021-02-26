using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityProject.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Web.CustomValidation
{
    public class CustomUserValidator:IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            string[] Digits = new string[] {"1", "2", "3", "4", "5", "6", "7", "8", "9"};

            foreach (var item in Digits)
            {
                if (user.UserName[1].ToString()==item)
                {
                    errors.Add(new IdentityError(){Code = "UsernameContainsFirstLetterDigitContain", Description = "The first letter of the username cannot be a digit." });

                }
                
            }
            if (errors.Count == 0)
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
