using System.ComponentModel.DataAnnotations;

namespace TUQA_Shop.DTOs
{
    public class CategoryRequest
    {
        [Required]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }

    }
}
