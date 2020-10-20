using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MVC.Api.Models;
using ReadLater.Data;
using ReadLater.Entities;
using ReadLater.Services;

namespace MVC.Api
{
    [RoutePrefix("api/Bookmarks")]
    public class BookmarksController : ApiController
    {
        private IBookmarkService _bookmarkService;
        private ICategoryService _categoryService;
        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        // GET: api/Bookmarks
        [HttpGet]
        [Route("GetBookmarks")]
        //[Route("api/bookmarks/{category}", Name ="GetBookmarks")]
        public IEnumerable<Bookmark> GetBookmarks(string category)
        {
            var bookmarks = _bookmarkService.GetBookmarks(category);
            return bookmarks;
        }

        // GET: api/Bookmarks/5
        [HttpGet]
        [Route("GetBookmark/{id}")]
        [ResponseType(typeof(Bookmark))]
        public IHttpActionResult GetBookmark(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmarkByID(id);
            if (bookmark == null)
            {
                return NotFound();
            }

            return Ok(bookmark);
        }

        // PUT: api/Bookmarks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBookmark(int id, Bookmark bookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookmark.ID)
            {
                return BadRequest();
            }

            _bookmarkService.UpdateBookmark(bookmark);

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Bookmarks
        [ResponseType(typeof(Bookmark))]
        public IHttpActionResult PostBookmark(Bookmark bookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bookmarkService.CreateBookmark(bookmark);

            return CreatedAtRoute("DefaultApi", new { id = bookmark.ID }, bookmark);
        }

        // DELETE: api/Bookmarks/5
        [ResponseType(typeof(Bookmark))]
        public IHttpActionResult DeleteBookmark(int id)
        {
            var bookmark = _bookmarkService.GetBookmarkByID(id);
            _bookmarkService.DeleteCategory(bookmark);
            if (bookmark == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}