﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hospins.Repository.Data;

namespace hospins.Models
{
    public class SubCategoryModel
    {
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "SubCategory is required.")]
        public string Name { get; set; } = null!;
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Sort order must be numeric with two decimal places only.")]
        [DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        public decimal? SortOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
