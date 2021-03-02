using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityProject.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Web.ClaimProviders
{
    public class ClaimProvider:IClaimsTransformation
    {
        private UserManager<AppUser> _userManager;

        public ClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal!=null&&principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
                AppUser user = await _userManager.FindByNameAsync(identity.Name);

                if (user!=null)
                {
                    //none
                }
            }
            return principal;
        }
    }
}
