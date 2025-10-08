namespace TUQA_Shop.models
{
    public enum OrderStatus
    {
        Pending,
        Cancelled,
        Approved,
        Shipped,
        Completed
    }
    public enum PaymentMethodType
    {
        Visa,Cash
    }
    public class Order
    {
        public int Id { get; set; }

        public string? SessionId { get; set; }

        public string? TransactionId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public decimal TotalPrice { get; set; }

        public PaymentMethodType PaymentMethodType { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }

        public string? Carrier { get; set; }
        public string? TrackingNumber { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }



    }
}
