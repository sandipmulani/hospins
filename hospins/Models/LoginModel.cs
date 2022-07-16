using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hospins.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email id is required.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Please enter a valid email id.")]
        public string Email { get; set; }

        public string ResetLink { get; set; }
    }

    public class ResetPasswordModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage="New password is required.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,}$", ErrorMessage = "New password must be 8 characters or longer with 1 lowercase, 1 uppercase, 1 numeric & 1 special character.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage="Confirm password is required.")]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password must be same.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
