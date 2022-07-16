using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class SalaryType
    {
        public int SalaryTypeId { get; set; }
        public string Name { get; set; } = null!;
        public decimal? SortOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
