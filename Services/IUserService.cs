using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public interface IUserService : IService<ApplicationUser>
    {
         Task<bool> ChangeRole(string userId, string roleName);

        Task<bool?> LockUnlock(string userId);
    }
}
