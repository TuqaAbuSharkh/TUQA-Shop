using Microsoft.AspNetCore.Identity;
using TUQA_Shop.Data;
using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public class UserService : Service<ApplicationUser> , IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public  UserService(ApplicationDbContext C, UserManager<ApplicationUser> userManager) : base(C)
        {
            context = C;
            this.userManager = userManager;
        }

        public async Task<bool> ChangeRole(string userId,string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                var oldRole = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, oldRole);

                var result = await userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return true;
                }
                else
                    return false;
            }
            return false;
        }

        public async Task<bool?> LockUnlock(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user is null) return null;

            var isLockedNow = user.LockoutEnabled && user.LockoutEnd > DateTime.Now;

            if (isLockedNow)
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.Now.AddMinutes(10);
            }
            await userManager.UpdateAsync(user);
            return !isLockedNow;
        }
    }
}
