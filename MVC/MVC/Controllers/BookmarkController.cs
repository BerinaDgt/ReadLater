using Microsoft.AspNet.Identity;
using MVC.Models;
using ReadLater.Entities;
using ReadLater.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class BookmarkController : Controller
    {
        private IBookmarkService _bookmarkService;
        private ICategoryService _categoryService;

        public BookmarkController(IBookmarkService bookmarkService, ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }
        // GET: Bookmark
        public ActionResult Index(string category)
        {
            var userId = User.Identity.GetUserId();
            List<Bookmark> model = _bookmarkService.GetBookmarks(category);
            var newList = model.Where(x => x.UserId == userId).ToList();
            return View(newList);
        }

        // GET: Bookmark/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var categories = _categoryService.GetCategoriesByUserId(userId);
            ViewBag.Categories = categories;
            return View();
        }

        // POST: Bookmark/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookmarkViewModel bookmarkViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                int categoryId = bookmarkViewModel.CategoryId ?? 0;

                var bookmark = new Bookmark
                {
                    UserId = userId,
                    URL = bookmarkViewModel.URL,
                    ShortDescription = bookmarkViewModel.ShortDescription,
                    CategoryId = categoryId
                };
                _bookmarkService.CreateBookmark(bookmark);
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Bookmark/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Categories = _categoryService.GetCategories();
            Bookmark bookmark = _bookmarkService.GetBookmarkByID((int)id);

            if (bookmark == null)
            {
                return HttpNotFound();
            }
            return View(bookmark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                _bookmarkService.UpdateBookmark(bookmark);
                return RedirectToAction("Index");
            }
            return View(bookmark);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmarkByID((int)id);
            if (bookmark == null)
            {
                return HttpNotFound();
            }
            return View(bookmark);
        }

        // POST: Bookmark/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmarkByID(id);
            _bookmarkService.DeleteCategory(bookmark);
            return RedirectToAction("Index");
        }

        public JsonResult AddCategory(string catName)
        {
            var userId = User.Identity.GetUserId();
            var newCategory = new Category
            {
                UserId = userId,
                Name = catName
            };
            _categoryService.CreateCategory(newCategory);

            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}