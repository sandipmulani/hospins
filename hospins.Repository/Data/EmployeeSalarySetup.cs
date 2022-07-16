using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class EmployeeSalarySetup
    {
        public int SalarySetupId { get; set; }
        public int EmployeeId { get; set; }
        public int SalaryTypeId { get; set; }
        public string? Basis { get; set; }
        public string Transport { get; set; } = null!;
        public string? Health { get; set; }
        public int? Pf { get; set; }
        public int? Tax { get; set; }
        public int? GrossSalary { get; set; }
        public int? Ctc { get; set; }
        public string? SalaryBenefits { get; set; }
        public string? BenefitsType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
