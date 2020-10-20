using ReadLater.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class BookmarkViewModel
    { 

        [StringLength(maximumLength: 500)]
        public string URL { get; set; }

        public string ShortDescription { get; set; }
        [Display(Name ="Category")]
        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreateDate { get; set; }
    }
    public class CreateBookmarkViewModel : BookmarkViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}