using System.Collections.Generic;
using ReadLater.Entities;

namespace ReadLater.Services
{
    public interface IBookmarkService
    {
        Bookmark GetBookmarkByID(int id);
        Bookmark CreateBookmark(Bookmark bookmark);
        void UpdateBookmark(Bookmark bookmark);
        List<Bookmark> GetBookmarks(string category);
        void DeleteCategory(Bookmark bookmark);
    }
}