namespace server_a.ApiModels
{
    public class Order 
    {
        public long? Id { get; set; }
        public long? SandwichId { get; set; }
        public StatusEnum? Status { get; set; }
    }

    public enum StatusEnum
    {

        Ordered,
        InQueue,
        Received,
        Ready,
        Failed
    }
}
