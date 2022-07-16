using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class EmployeeAddress
    {
        public int AddressId { get; set; }
        public int EmployeeId { get; set; }
        public string Address { get; set; } = null!;
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string City { get; set; } = null!;
        public string? ZipCode { get; set; }
        public int AddressTypeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
