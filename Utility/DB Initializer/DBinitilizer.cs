<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
using TUQA_Shop.Data;
using TUQA_Shop.models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

=======
﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TUQA_Shop.Data;
using TUQA_Shop.models;
>>>>>>> c834fd62c84bfde81c178f6e24c295094fbd524a

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

<<<<<<< HEAD
            if (roleManager.Roles == null)
=======
            if (roleManager.Roles.IsNullOrEmpty())
>>>>>>> c834fd62c84bfde81c178f6e24c295094fbd524a
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
