using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hospins.Models
{
    public class LogbookModel
    {
        public int? SequnceId { get; set; }
        public int LogbookId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "{0} is required.")]
        public DateTime Date { get; set; }

        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string? Description { get; set; }
        public int? AssisgnTo { get; set; }
        public int? PriorityId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
