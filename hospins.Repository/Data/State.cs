using System;
using System.Collections.Generic;

namespace hospins.Repository.Data
{
    public partial class State
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string? Name { get; set; }
    }
}
