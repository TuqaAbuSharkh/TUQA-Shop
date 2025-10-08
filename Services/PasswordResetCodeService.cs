using TUQA_Shop.Data;
using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public class PasswordResetCodeService : Service<PasswordResetCode> , IPasswordRessetCodeService
    {
        private readonly ApplicationDbContext context;

        public PasswordResetCodeService(ApplicationDbContext C) : base(C)
        {
            context = C;
        }
    }
}
