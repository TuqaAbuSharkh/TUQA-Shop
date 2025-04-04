using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TUQA_Shop.Data;
using TUQA_Shop.models;

namespace TUQA_Shop.Services
{
    public class BrandService :IBrandService
    {
        ApplicationDbContext context;

        public BrandService(ApplicationDbContext C)
        {
            context = C;
        }

        Brand IBrandService.Add(Brand brand)
        {
            context.Brands.Add(brand);
            context.SaveChanges();

            return brand;
        }

        bool IBrandService.Delete(int id)
        {
            Brand? brand = context.Brands.Find(id);
            if (brand is null)
                return false;

            context.Brands.Remove(brand);
            context.SaveChanges();
            return true;
        }

        Brand IBrandService.Get(Expression<Func<Brand, bool>> expression)
        {
            return context.Brands.FirstOrDefault(expression);
        }

        IEnumerable<Brand> IBrandService.GetAll()
        {
            return context.Brands.ToList();
        }

        bool IBrandService.update(int id, Brand brand)
        {
            Brand? brandInDatabase = context.Brands.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (brandInDatabase is null)
                return false;

            brand.Id = id;
            context.Brands.Update(brand);
            context.SaveChanges();
            return true;

        }
    }
}
