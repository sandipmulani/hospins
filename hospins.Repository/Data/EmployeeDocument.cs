using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class EmployeeDocument
    {
        public int EmployeeDocumentId { get; set; }
        public int EmployeeId { get; set; }
        public int DocumentTypeId { get; set; }
        public string FileName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
