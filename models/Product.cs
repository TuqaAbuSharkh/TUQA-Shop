namespace TUQA_Shop.models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public IFormFile mainImg { get; set; }
        public int Quantity { get; set; }
        public double Rate{ get; set; }
        public bool Status { get; set; }
        public Category category { get; set; }

        public int CategoryId { get; set; }


    }
}
