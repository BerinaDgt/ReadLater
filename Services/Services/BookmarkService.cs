﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadLater.Entities;
using ReadLater.Repository;

namespace ReadLater.Services
{
    public class BookmarkService : IBookmarkService
    {
        protected IUnitOfWork _unitOfWork;

        public BookmarkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Bookmark CreateBookmark(Bookmark bookmark)
        {
            bookmark.CreateDate = DateTime.Now;
            _unitOfWork.Repository<Bookmark>().Insert(bookmark);
            _unitOfWork.Save();
            return bookmark;
        }

        public void DeleteCategory(Bookmark bookmark)
        {
            _unitOfWork.Repository<Bookmark>().Delete(bookmark);
            _unitOfWork.Save();
        }

        public Bookmark GetBookmarkByID(int id)
        {
            return _unitOfWork.Repository<Bookmark>().Query()
                                                   .Filter(c => c.ID == id)
                                                   .Get()
                                                   .FirstOrDefault();
        }

        public List<Bookmark> GetBookmarks(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return _unitOfWork.Repository<Bookmark>().Query()
                                                        .OrderBy(l => l.OrderByDescending(b => b.CreateDate))
                                                        .Get()                                                        
                                                        .ToList();
            }
            else
            {
                return _unitOfWork.Repository<Bookmark>().Query()
                                                            .Filter(b => b.Category != null && b.Category.Name == category)                                        
                                                            .Get()
                                                            .ToList();
            }
        }

        public void UpdateBookmark(Bookmark bookmark)
        {
            var dbBookmark = GetBookmarkByID(bookmark.ID);
            if (dbBookmark !=null)
            {
                dbBookmark.URL = bookmark.URL;
                dbBookmark.ShortDescription = bookmark.ShortDescription;
                dbBookmark.CategoryId = bookmark.CategoryId;
                _unitOfWork.Repository<Bookmark>().Update(dbBookmark);
                _unitOfWork.Save();
            }
        }
    }
}
