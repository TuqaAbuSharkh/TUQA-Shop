using Microsoft.AspNetCore.Identity;

namespace TUQA_Shop.models
{
    public enum UserGender
    {
        Male,Female
    }
    public class ApplicationUser : IdentityUser
    {
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public UserGender Gender { get; set; }
        public DateTime BirthOfDate { get; set; }


    }
}
