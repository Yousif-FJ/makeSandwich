using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server_a.ApiModels;
using server_a.Helpers;

namespace server_a.HostedService;

public class OrderStatusUpdater(ILogger<OrderStatusUpdater> logger, IConnection mqConnection,
            OrdersCollection orders) : IHostedService
{
    private IModel? _mqChannel;
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _mqChannel = mqConnection.CreateModel();

        _mqChannel.ExchangeDeclare("orderStatusUpdates", ExchangeType.Direct);
        _mqChannel.QueueDeclare("orderStatusUpdates", false, false, false, null);
        _mqChannel.QueueBind("orderStatusUpdates", "orderStatusUpdates", "orderStatus");


        var consumer = new EventingBasicConsumer(_mqChannel);
        _mqChannel.BasicConsume("orderStatusUpdates", true, consumer);
        consumer.Received += OnOrderUpdateReceived;

        return Task.CompletedTask;
    }

    private void OnOrderUpdateReceived(object? model, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var receivedMessage = Encoding.UTF8.GetString(body);
        logger.LogInformation("Received message: {message}", receivedMessage);
        var order = JsonSerializer.Deserialize<Order>(receivedMessage)!;

        var existingOrder = orders.FirstOrDefault(o => o.Id == order.Id);
        if (existingOrder != null)
        {
            existingOrder.Status = order.Status;
        }
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _mqChannel?.Dispose();
        return Task.CompletedTask;
    }
}