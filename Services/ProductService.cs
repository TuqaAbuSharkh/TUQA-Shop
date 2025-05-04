using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using TUQA_Shop.Data;
using TUQA_Shop.DTOs;
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
        Product IProductService.Add(ProductRequest productR)
        {
            var file = productR.mainImg;
            var product = productR.Adapt<Product>();
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
               product.mainImg = fileName;
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

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", product.mainImg);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            context.Products.Remove(product);
            context.SaveChanges();
            return true;
        }

        Product IProductService.Get(Expression<Func<Product, bool>> expression)
        {
            return context.Products.FirstOrDefault(expression);
        }

        IEnumerable<Product> IProductService.GetAll([FromQuery] string? query, [FromQuery]int page, [FromQuery]int limit=10)
        {
            IQueryable<Product> products = context.Products;
            if (query != null)
                products = products.Where(p => p.Name.Contains(query) || p.Description.Contains(query));

            if(page<=0|| limit<=0)
            {
                page = 1;
                limit = 10;
            }
            products = products.Skip((page - 1) * limit).Take(limit);

            return products;
            }

        bool IProductService.update([FromRoute] int id, [FromForm] ProductUpdateRequest productR)
        {
            var productInDatabase = context.Products.AsNoTracking().FirstOrDefault(c => c.Id == id);
            var product = productInDatabase.Adapt<Product>();
            var file = productR.mainImg;

            if (productInDatabase != null)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", productInDatabase.mainImg);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                    product.mainImg = fileName;
                }
                else
                    product.mainImg = productInDatabase.mainImg;

                product.Id = id;
                context.Products.Update(product);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
