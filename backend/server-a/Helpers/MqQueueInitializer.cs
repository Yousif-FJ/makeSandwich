using RabbitMQ.Client;

namespace server_a.Helpers;

public static class MqQueueInitializer
{
    public static void EnsureOrdersQueueCreated(this IConnection mqConnection)
    {
        using var mqChannel = mqConnection.CreateModel();
        mqChannel.ExchangeDeclare("orders", ExchangeType.Direct);
        mqChannel.QueueDeclare("orders", true, false, false, null);
        mqChannel.QueueBind("orders", "orders", "order");
    }
}
