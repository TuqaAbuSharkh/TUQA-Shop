using Microsoft.EntityFrameworkCore;
using TUQA_Shop.Data;
using TUQA_Shop.models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;


namespace TUQA_Shop.Utility.DB_Initializer
{
    public class DBinitilizer : IDBinitilizer
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public DBinitilizer(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        
        public async Task initilize()
        {
            try
            {
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (roleManager.Roles == null)
            {
                await roleManager.CreateAsync(new ("SuperAdmin"));
                await roleManager.CreateAsync(new("Admin"));
                await roleManager.CreateAsync(new("Customer"));
                await roleManager.CreateAsync(new("Company"));


                await userManager.CreateAsync(new()
                {
                    FirstName = "super",
                    LastName = "admin",
                    Gender = UserGender.Female,
                    BirthOfDate = new DateTime(2005, 1, 29),
                    Email = "admin@tshop.com"
                }, "admin@34");

                var user =await userManager.FindByEmailAsync("admin@tshop.com");
                await userManager.AddToRoleAsync(user,"SuperAdmin");
            }
           
            
        }
    }
}
