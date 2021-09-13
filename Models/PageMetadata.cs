using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FletcherProj.Models
{
    public class PageMetadata
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
    }
}
