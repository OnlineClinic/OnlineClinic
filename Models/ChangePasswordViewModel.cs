using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class ChangePasswordViewModel
    {

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ControllerName { get; set; }

        [Required(ErrorMessage = "Please enter your old password")]
        public string OldPassword { get; set; }



        public Guid Loginid { get; set; }

        public ChangePasswordViewModel()
        {

        }
        public ChangePasswordViewModel(ChangePasswordViewModel Model)
        {
            this.Loginid = Loginid;
            this.Password = Password;
            this.ConfirmPassword = ConfirmPassword;
            this.OldPassword = OldPassword;
        }

    }
}