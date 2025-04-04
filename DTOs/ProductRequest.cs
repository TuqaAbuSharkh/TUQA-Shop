using TUQA_Shop.models;

namespace TUQA_Shop.DTOs
{
    public class ProductRequest
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public IFormFile mainImg { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public bool Status { get; set; }
        public Category category { get; set; }

        public int CategoryId { get; set; }
    }
}
