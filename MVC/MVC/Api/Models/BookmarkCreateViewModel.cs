using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Api.Models
{
    public class BookmarkCreateViewModel
    {
        public string URL { get; set; }
        public string ShortDescription { get; set; }
        public int? CategoryId { get; set; }
    }
}