using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.Web.ViewModels
{
    public class PasswordChangedViewModel
    {
        [Display(Name = "Current Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string PasswordCurrent { get; set; }
        [Display(Name = "Enter New Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string PasswordNew { get; set; }
        [Display(Name = "Confirm New Password")]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [Compare("PasswordNew")]
        public string PasswordConfirm { get; set; }

    }
}
