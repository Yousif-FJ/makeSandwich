using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server_a.Data.Collections;
using server_a.Data.Models;
using server_a.RealTime;

namespace server_a.HostedService;

public class OrderStatusUpdater(ILogger<OrderStatusUpdater> logger, IConnection mqConnection,
            OrdersCollection orders, IHubContext<OrderStatusHub> rtHubContext) : IHostedService
{
    private IModel? _mqChannel;
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _mqChannel = mqConnection.CreateModel();

        _mqChannel.ExchangeDeclare("orderStatusUpdates", ExchangeType.Direct);
        _mqChannel.QueueDeclare("orderStatusUpdates", true, false, false, null);
        _mqChannel.QueueBind("orderStatusUpdates", "orderStatusUpdates", "orderStatus");


        var consumer = new EventingBasicConsumer(_mqChannel);
        _mqChannel.BasicConsume("orderStatusUpdates", false, consumer);
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
            rtHubContext.Clients.All.SendAsync("OrderStatusUpdated");
        }
        _mqChannel!.BasicAck(ea.DeliveryTag, false); 
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _mqChannel?.Dispose();
        return Task.CompletedTask;
    }
}