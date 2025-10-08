namespace TUQA_Shop.models
{
    public class Review
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Rate { get; set; }
        public string? Comment { get; set; }

        public ICollection<ReviewImages> ReviewImages { get; } = new List<ReviewImages>();

        public DateTime ReviewDate { get; set; }


    }
}
