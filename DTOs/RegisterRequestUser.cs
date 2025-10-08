using System.ComponentModel.DataAnnotations;
using TUQA_Shop.models;
using TUQA_Shop.Validation;

namespace TUQA_Shop.DTOs
{
    public class RegisterRequestUser
    {
        [MinLength(4)]
        public string FirstName { get; set; }
        [MinLength(4)]

        public string LastName { get; set; }
        [MinLength(6)]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password),ErrorMessage ="Message do not match!")]
        public string ConfirmPassword { get; set; }
        public UserGender Gender { get; set; }
        [Over18Years]
        public DateTime BirthOfDate { get; set; }

    }
}
