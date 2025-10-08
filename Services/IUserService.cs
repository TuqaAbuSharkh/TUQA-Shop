using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public interface IUserService : IService<ApplicationUser>
    {
         Task<bool> ChangeRole(string userId, string roleName);
<<<<<<< HEAD

        Task<bool?> LockUnlock(string userId);
=======
>>>>>>> c834fd62c84bfde81c178f6e24c295094fbd524a
    }
}
