using ReadLater.Entities;
using SimpleInjector.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MVC.Api.Models
{
    public class BookmarkViewModel
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string ShortDescription { get; set; }
        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreateDate { get; set; }
        public static Expression<Func<Bookmark, BookmarkViewModel>> Projection
        {
            get {
                return x => new BookmarkViewModel()
                { 
                    Id = x.ID,
                    URL = x.URL,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    CreateDate = x.CreateDate,
                    ShortDescription= x.ShortDescription
                };
            }
        }

    }
}