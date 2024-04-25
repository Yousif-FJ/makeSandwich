using RabbitMQ.Client;

namespace server_a.Helpers;

public static class MqConnectionCreator
{
    public static IConnection CreateMqConnection(IConfiguration configuration)
    {
        var rabbitMqHost = configuration.GetValue<string>("RabbitMQ_Host");
        var connection = new ConnectionFactory() { HostName = rabbitMqHost }.CreateConnection();

        connection.EnsureOrdersQueueCreated();

        return connection;
    }
}