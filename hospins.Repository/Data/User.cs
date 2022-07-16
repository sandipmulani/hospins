using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
