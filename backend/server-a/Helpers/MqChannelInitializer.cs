using RabbitMQ.Client;

namespace server_a.Helpers;

public static class MqChannelInitializer
{
    public static void EnsureOrdersQueueCreated(this IConnection mqConnection)
    {
        try
        {
            using var mqChannel = mqConnection.CreateModel();
            mqChannel.QueueBind("orders", "orders", "order");
        }
        catch (RabbitMQ.Client.Exceptions.OperationInterruptedException)
        {
            using var mqChannel = mqConnection.CreateModel();
            mqChannel.ExchangeDeclare("orders", ExchangeType.Direct);
            mqChannel.QueueDeclare("orders", false, false, false, null);
        }
    }
}
