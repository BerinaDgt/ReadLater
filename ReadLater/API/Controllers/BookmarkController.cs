using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadLater.Services;

namespace API.Controllers
{
    [Route("api/bookmark")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        IBookmarkService _bookmarkService;
        ICategoryService _categoryService;

        public BookmarkController(IBookmarkService bookmarkService, ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookmarkModel>> Get(string category)
        {
            var model = _bookmarkService.GetBookmarks(category)
                .Select(x=> new BookmarkModel()
                { 
                    URL= x.URL,
                    ShortDescription = x.ShortDescription,
                    Category = x.Category.Name,
                    CreateDate = x.CreateDate
                });
            return Ok(model);
        }
    }
}
