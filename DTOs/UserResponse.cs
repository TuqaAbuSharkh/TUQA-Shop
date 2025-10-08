using TUQA_Shop.models;

namespace TUQA_Shop.DTOs
{
    public class UserResponse
    {
        public string? Id {get;set;}
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Gender { get; set; }
        public DateTime BirthOfDate { get; set; }
    }
}
