using TUQA_Shop.models;

namespace TUQA_Shop.DTOs
{
    public class ReviewRequest
    {

        public string ApplicationUserId { get; set; }

        public int Rate { get; set; }
        public string? Comment { get; set; }

        public ICollection<ReviewImageRequest>? ReviewImages { get; } = new List<ReviewImageRequest>();

        public DateTime ReviewDate { get; set; }
    }
}
