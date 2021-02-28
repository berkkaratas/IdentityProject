using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Web.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Picture { get; set; }


    }
}
