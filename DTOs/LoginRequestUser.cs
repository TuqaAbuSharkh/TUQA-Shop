using System.ComponentModel.DataAnnotations;

namespace TUQA_Shop.DTOs
{
    public class LoginRequestUser
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

        public bool RemmberMe { get; set; }

    }
}
