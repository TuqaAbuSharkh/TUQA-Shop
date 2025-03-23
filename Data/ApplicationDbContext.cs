using Microsoft.EntityFrameworkCore;
using TUQA_Shop.models;

namespace TUQA_Shop.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

       public DbSet<Category> Categories { get; set; }
    }
}
