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

        // GET: api/Bookmarks/5
        [HttpGet]
        [Route("GetBookmark/{id}")]
        [ResponseType(typeof(BookmarkViewModel))]
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

        // GET: api/Bookmarks
        [HttpGet]
        [Route("GetAll")]
        [ResponseType(typeof(Bookmark))]
        public async Task<IEnumerable<BookmarkViewModel>> GetAll()
        {
            var bookmarks = await _context.Bookmarks
                .Select(BookmarkViewModel.Projection)
                .ToListAsync();
            return bookmarks;
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
    }
}