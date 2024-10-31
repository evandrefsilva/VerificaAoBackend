using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class PaginationParameters
    {
        public int page { get; set; } = 1;
        public int take { get; set; } = 30;
    }
    public class PaginationFilterParameters : PaginationParameters
    {
        public string? filter { get; set; } = "";
    }
}
