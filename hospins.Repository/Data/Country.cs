using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class Country
    {
        public int CountryId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}
