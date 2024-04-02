namespace server_a.ApiModels
{
    public partial class Order 
    {
        public long? Id { get; set; }
        public long? SandwichId { get; set; }
        public StatusEnum? Status { get; set; }
    }

    public enum StatusEnum
    {

        Ordered = 1,
        Received = 2,
        InQueue = 3,

        Ready = 4,

        Failed= 5
    }
}

