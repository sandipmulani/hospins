using System;
using System.ComponentModel.DataAnnotations;

namespace hospins.Models
{
    public class RoleModel
    {
        [Required(ErrorMessage = "Role is required.")]
        public string RoleName { get; set; }

        public int RoleId { get; set; }

        public Boolean IsActive { get; set; }

        public int UserCount { get; set; }
    }
}