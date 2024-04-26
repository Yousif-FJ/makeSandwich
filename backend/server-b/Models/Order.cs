namespace server_b.Models
{
    public class Order
    {
        public long? Id { get; set; }
        public long? SandwichId { get; set; }
        public StatusEnum? Status { get; set; }
    }

    public enum StatusEnum
    {

        Ordered = 0,
        InQueue = 1,
        Received = 2,
        Ready = 3,
        Failed = 4
    }
}

