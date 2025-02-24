﻿using System;
using System.Collections.Generic;
using ReadLater.Entities;
namespace ReadLater.Services
{
    public interface ICategoryService
    {
        Category CreateCategory(Category category);
        List<Category> GetCategories();
        Category GetCategory(int Id);
        List<Category> GetCategoriesByUserId(string userId);
        Category GetCategory(string Name);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
