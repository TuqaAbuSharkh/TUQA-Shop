using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using TUQA_Shop.Data;
using TUQA_Shop.models;

namespace TUQA_Shop.Services
{
    public class ProductService : IProductService
    {
        ApplicationDbContext context;
        public ProductService(ApplicationDbContext C)
        {
            context = C;
        }
        Product IProductService.Add(Product product)
        {
            var file = product.mainImg;
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
               // product.mainImg = fileName;
                context.Products.Add(product);
                context.SaveChanges();
            }
            return product;
        }

        bool IProductService.Delete(int id)
        {
            Product? product = context.Products.Find(id);
            if (product is null)
                return false;

           // var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", product.mainImg);
            //if (System.IO.File.Exists(filePath))
            //{
            //    System.IO.File.Delete(filePath);
            //}
            context.Products.Remove(product);
            context.SaveChanges();
            return true;
        }

        Product IProductService.Get(Expression<Func<Product, bool>> expression)
        {
            return context.Products.FirstOrDefault(expression);
        }

        IEnumerable<Product> IProductService.GetAll()
        {
            return context.Products.ToList();
        }

        bool IProductService.update(int id, Product product)
        {
            Product? productInDatabase = context.Products.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (productInDatabase is null)
                return false;

            product.Id = id;
            context.Products.Update(product);
            context.SaveChanges();
            return true;
        }
    }
}
