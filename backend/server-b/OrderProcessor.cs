using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server_b.Models;

namespace server_b;

public class SandwichMaker(ILogger<SandwichMaker> logger, ConnectionFactory rabbitFactory) 
    : IHostedService
{
    private readonly ILogger<SandwichMaker> _logger = logger;
    private IConnection? _rabbitConnection;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rabbitConnection = rabbitFactory.CreateConnection();
        var rabbitChannel = _rabbitConnection.CreateModel();
        rabbitChannel.QueueDeclare("orders", false, false, false, null);

        var consumer = new EventingBasicConsumer(rabbitChannel);
        consumer.Received += OnOrderReceived;
        //The autoAck parameter is set to true, in real life scenarios this should be set to false.
        //Failing orders should be sent to dead letter exchange and then processed separately.
        rabbitChannel.BasicConsume("orders", true, consumer);


        rabbitChannel.ExchangeDeclare("orderStatusUpdates", ExchangeType.Direct);
        rabbitChannel.QueueDeclare("orderStatusUpdates", false, false, false, null);
        rabbitChannel.QueueBind("orderStatusUpdates", "orderStatusUpdates", "orderStatus");
        
        return Task.CompletedTask;
    }

    private void OnOrderReceived(object? model, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var receivedMessage = Encoding.UTF8.GetString(body);
        _logger.LogInformation("Received message: {message}", receivedMessage);
        var order = JsonSerializer.Deserialize<Order>(receivedMessage)!;

        order.Status = StatusEnum.Received;
        SendOrderToOrderStatusQueue(order);

        Task.Delay(5000).GetAwaiter().GetResult();

        order.Status = StatusEnum.Ready;
        SendOrderToOrderStatusQueue(order);
    }

    private void SendOrderToOrderStatusQueue(Order order)
    {
        var rabbitChannel = _rabbitConnection!.CreateModel();
        var statusMessage = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order));
        rabbitChannel.BasicPublish("orderStatusUpdates", "orderStatus", null, statusMessage);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _rabbitConnection?.Dispose();
        return Task.CompletedTask;
    }
}
