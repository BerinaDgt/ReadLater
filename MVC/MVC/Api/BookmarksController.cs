using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using MVC.Api.Models;
using ReadLater.Data;
using ReadLater.Entities;
using ReadLater.Services;

namespace MVC.Api
{
    [Authorize]
    public class BookmarksController : ApiController
    {
        private IBookmarkService _bookmarkService;
        private ICategoryService _categoryService;
        private ReadLaterDataContext _context;
        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService, ReadLaterDataContext context)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
            _context = context;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        // GET: api/Bookmarks
        [HttpGet]
        [Route("GetBookmarks")]
        //[Route("api/bookmarks/{category}", Name ="GetBookmarks")]
        public async Task<IEnumerable<BookmarkViewModel>> GetBookmarks()
        {
            var userId = RequestContext.Principal.Identity.GetUserId();
            var bookmarks = await _context.Bookmarks
                .Select(BookmarkViewModel.Projection)
                .ToListAsync();
            return bookmarks;
        }

        // GET: api/Bookmarks/5
        [HttpGet]
        [Route("GetBookmark/{id}")]
        [ResponseType(typeof(Bookmark))]
        public async Task<IHttpActionResult> GetBookmark(int id)
        {
            var bookmark = await _context.Bookmarks
                .Where(x=>x.ID == id)
                .Select(BookmarkViewModel.Projection)
                .SingleOrDefaultAsync();
            if (bookmark == null)
                return BadRequest("No Bookmark found!");

            return Ok(bookmark);
        }

        // PUT: api/Bookmarks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBookmark(BookmarkEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookmark = await _context.Bookmarks
                .Where(x => x.ID == model.Id)
                .SingleOrDefaultAsync();
            if (bookmark ==null)
                return BadRequest("Bookmark not found!");

            var bookmarkExists = _context.Bookmarks
                    .Any(x => 
                        x.URL == model.URL &&
                        x.ShortDescription == model.ShortDescription &&
                        x.CategoryId == model.CategoryId);
            if (bookmarkExists)
            {
                return BadRequest("Bookmark already exists");
            }

            if (model.CategoryId !=null)
            {
                var category = _context.Categories.SingleOrDefault(x => x.ID == model.CategoryId);
                if (category == null)
                {
                    return BadRequest("Category not found");
                }
            }
            var upsertBookmark = new Bookmark()
            {
                ID = model.Id,
                ShortDescription= model.ShortDescription,
                URL = model.URL,
                CategoryId = model.CategoryId
            };
            
            _bookmarkService.UpdateBookmark(upsertBookmark);

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Bookmarks
        [ResponseType(typeof(Bookmark))]
        public IHttpActionResult PostBookmark(BookmarkCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.URL == null || model.URL== string.Empty)
                return BadRequest("Url is required !");

            if (model.ShortDescription == null || model.ShortDescription == string.Empty)
                return BadRequest("Description is required !");

            var bookmarkExists = _context.Bookmarks
                    .Any(x =>
                        x.URL == model.URL &&
                        x.ShortDescription == model.ShortDescription &&
                        x.CategoryId == model.CategoryId);
            if (bookmarkExists)
            {
                return BadRequest("Bookmark already exists");
            }
            var newBookmark = new Bookmark()
            { 
                URL = model.URL,
                ShortDescription = model.ShortDescription,
                CategoryId = model.CategoryId
            };
            var bookmark= _bookmarkService.CreateBookmark(newBookmark);


            return CreatedAtRoute("DefaultApi", new { id = bookmark.ID }, bookmark);
        }

        // DELETE: api/Bookmarks/5
        [ResponseType(typeof(Bookmark))]
        public IHttpActionResult DeleteBookmark(int id)
        {
            var bookmark = _context.Bookmarks
                .Where(x => x.ID == id)
                .SingleOrDefault();
            if (bookmark == null)
                return BadRequest("Bookmark not found!");
           
            _bookmarkService.DeleteCategory(bookmark);

            return Ok();
        }
    }
}