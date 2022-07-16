using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public int DesignationId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? BloodGroup { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Picture { get; set; }
        public int? CountryId { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
