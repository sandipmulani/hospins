using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class EmployeeHistory
    {
        public int EmployeeHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public string? CompanyName { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? ReleasingDate { get; set; }
        public int? DesignationId { get; set; }
        public string? LastSalary { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
