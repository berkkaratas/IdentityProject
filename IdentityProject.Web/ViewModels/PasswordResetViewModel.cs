using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Web.ViewModels
{
    public class PasswordResetViewModel
    {
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "New Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string PasswordNew { get; set; }
    }
}
