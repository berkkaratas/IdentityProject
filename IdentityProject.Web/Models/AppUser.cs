﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Web.Models
{
    public class AppUser : IdentityUser
    {
        public string Picture { get; set; }

    }
}
