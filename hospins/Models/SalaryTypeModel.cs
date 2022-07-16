using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hospins.Repository.Data;

namespace hospins.Models
{
    public class SalaryTypeModel
    {
        public int SalaryTypeId { get; set; }

        [Required(ErrorMessage = "Salary type is required.")]
        public string Name { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Sort order must be numeric with two decimal places only.")]
        [DisplayFormat(DataFormatString="{0:0.##}",ApplyFormatInEditMode = true)]
        public decimal? SortOrder { get; set; }

        public bool IsActive { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}