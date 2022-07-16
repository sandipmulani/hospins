using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class Logbook
    {
        public int LogbookId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string? Description { get; set; }
        public int? AssisgnTo { get; set; }
        public int? PriorityId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
