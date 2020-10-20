using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class BookmarkModel
    {
        public string URL { get; set; }
        public string ShortDescription { get; set; }
        public string Category { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
