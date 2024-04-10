using RabbitMQ.Client;

namespace server_a.Helpers;

public static class MqChannelInitializer
{
    public static void EnsureOrdersQueueCreated(this IConnection mqConnection)
    {
        var mqChannel = mqConnection.CreateModel();
        try
        {
            mqChannel.QueueBind("orders", "orders", "order");
        }
        catch (RabbitMQ.Client.Exceptions.OperationInterruptedException)
        {
            mqChannel = mqConnection.CreateModel();
            mqChannel.ExchangeDeclare("orders", ExchangeType.Direct);
            mqChannel.QueueDeclare("orders", false, false, false, null);
        }
    }
}
