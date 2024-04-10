using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server_a.ApiModels;
using server_a.Helpers;

namespace server_a.BackgroundWorkers;

public class OrderStatusUpdater(ILogger<OrderStatusUpdater> logger, ConnectionFactory rabbitFactory,
            OrdersCollection orders) : IHostedService
{
    private IConnection? _rabbitConnection;
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rabbitConnection = rabbitFactory.CreateConnection();
        var rabbitChannel = _rabbitConnection.CreateModel();

        rabbitChannel.ExchangeDeclare("orderStatusUpdates", ExchangeType.Direct);
        rabbitChannel.QueueDeclare("orderStatusUpdates", false, false, false, null);
        rabbitChannel.QueueBind("orderStatusUpdates", "orderStatusUpdates", "orderStatus");


        var consumer = new EventingBasicConsumer(rabbitChannel);
        rabbitChannel.BasicConsume("orderStatusUpdates", true, consumer);
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
        _rabbitConnection?.Dispose();
        return Task.CompletedTask;
    }
}