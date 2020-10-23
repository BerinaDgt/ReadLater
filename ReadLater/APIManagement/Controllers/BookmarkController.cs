using APIManagement.Models;
using ReadLater.Data;
using ReadLater.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace APIManagement.Controllers
{
    public class BookmarkController : ApiController
    {
        private IBookmarkService _bookmarkService;
        private ICategoryService _categoryService;
        private ReadLaterDataContext _context;
        public BookmarkController(IBookmarkService bookmarkService, ICategoryService categoryService, ReadLaterDataContext context)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("GetBookmark/{id}")]
        [ResponseType(typeof(BookmarkViewModel))]
        public async Task<IHttpActionResult> GetBookmark(int id)
        {
            var bookmark = await _context.Bookmarks
                .Where(x => x.ID == id)
                .Select(BookmarkViewModel.Projection)
                .SingleOrDefaultAsync();
            if (bookmark == null)
                return BadRequest("No Bookmark found!");

            return Ok(bookmark);
        }

    }
}
