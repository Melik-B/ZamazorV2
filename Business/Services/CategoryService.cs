using AppCore.Business.Models.Results;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.DataAccess.EntityFramework;
using Business.Models;
using Business.Services.Bases;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Contexts;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {
        public RepoBase<Category, ZamazorContext> Repo { get; set; } = new Repo<Category, ZamazorContext>();

        public Result Add(CategoryModel model)
        {
            if (Repo.Query().Any(category => category.Name.ToUpper() == model.Name.ToUpper().Trim()))
                return new ErrorResult("A record with the entered category name already exists!");

            Category entity = new Category()
            {
                Name = model.Name.Trim(),
                Description = model.Description?.Trim()
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            Category entity = Repo.Query(category => category.Id == id, "Products").SingleOrDefault();
            if (entity.Products != null && entity.Products.Count > 0)
            {
                return new ErrorResult("Category cannot be deleted because it has associated products!");
            }
            Repo.Delete(entity);
            return new SuccessResult("Category has been successfully deleted.");
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<CategoryModel> Query()
        {
            IQueryable<CategoryModel> query = Repo.Query("Products").OrderBy(category => category.Name).Select(category => new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ProductCountDisplay = category.Products.Count
            });
            return query;
        }

        public Result Update(CategoryModel model)
        {
            if (Repo.Query().Any(category => category.Name.ToUpper() == model.Name.ToUpper().Trim() && category.Id != model.Id))
                return new ErrorResult("A record with the entered category name already exists!");

            Category entity = Repo.Query().SingleOrDefault(category => category.Id == model.Id);
            entity.Name = model.Name.Trim();
            entity.Description = model.Description?.Trim();
            Repo.Update(entity);
            return new SuccessResult("Category has been successfully updated.");
        }

        public async Task<Result<List<CategoryModel>>> GetCategoriesAsync()
        {
            List<CategoryModel> categories = await Query().ToListAsync();
            return new SuccessResult<List<CategoryModel>>(categories);
        }
    }
}
