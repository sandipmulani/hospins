using System.ComponentModel.DataAnnotations;

namespace hospins.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select role.")]
        [Range(0, int.MaxValue, ErrorMessage = "Please Select Role.")]
        public int RoleId { get; set; }

        public string Role { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,}$", ErrorMessage = "Password must be 8 characters or longer with 1 lowercase, 1 uppercase, 1 numeric & 1 special character.")]
        public string Password { get; set; }

        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,}$", ErrorMessage = "Password must be 8 characters or longer with 1 lowercase, 1 uppercase, 1 numeric & 1 special character.")]
        public string UpdatePassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Password and confirm password must be same.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; }

        public string Mobile { get; set; }
    }
}