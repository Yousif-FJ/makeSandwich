using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using server_b.Models;

namespace server_b;

public class OrderProcessor(ILogger<OrderProcessor> logger, IConnection mqConnection) 
    : IHostedService
{
    private readonly ILogger<OrderProcessor> _logger = logger;
    private readonly IConnection _mqConnection = mqConnection;
    private IModel? _mqChannel;


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _mqChannel = _mqConnection.CreateModel();
        _mqChannel.ExchangeDeclare("orders", ExchangeType.Direct);
        _mqChannel.QueueDeclare("orders", true, false, false, null);
        _mqChannel.QueueBind("orders", "orders", "order");


        var consumer = new EventingBasicConsumer(_mqChannel);
        consumer.Received += OnOrderReceived;
        _mqChannel.BasicConsume("orders", false, consumer);
        _mqChannel.ExchangeDeclare("orderStatusUpdates", ExchangeType.Direct);
        _mqChannel.QueueDeclare("orderStatusUpdates", true, false, false, null);
        _mqChannel.QueueBind("orderStatusUpdates", "orderStatusUpdates", "orderStatus");
        
        return Task.CompletedTask;
    }

    private void OnOrderReceived(object? model, BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var receivedMessage = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Received message: {message}", receivedMessage);
            var order = JsonSerializer.Deserialize<Order>(receivedMessage)!;
            if (order.Id == null || order.SandwichId == null)
            {
                throw new Exception("Invalid order received.");
            }
            _mqChannel!.BasicAck(ea.DeliveryTag, false); 

            order.Status = StatusEnum.Received;
            SendOrderToOrderStatusQueue(order);


            Task.Delay(5000).GetAwaiter().GetResult();

            order.Status = StatusEnum.Ready;
            SendOrderToOrderStatusQueue(order);
        }
        catch (Exception e)
        {
            using var channel = _mqConnection.CreateModel();
            _logger.LogError(e, "Error processing order. Message was sent to dead letter queue.");

            channel.QueueDeclare("deadLetter", true, false, false, null);
            channel.BasicPublish("", "deadLetter", null, ea.Body);
            channel.BasicAck(ea.DeliveryTag, false);           
        }
    }

    private void SendOrderToOrderStatusQueue(Order order)
    {
        var orderText = JsonSerializer.Serialize(order);
        var statusMessage = Encoding.UTF8.GetBytes(orderText);
        _mqChannel!.ConfirmSelect();
        _mqChannel.BasicPublish("orderStatusUpdates", "orderStatus", null, statusMessage);
        _mqChannel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));
        _logger.LogInformation("Sent order status. OrderId: {message}", orderText);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _mqChannel?.Dispose();
        return Task.CompletedTask;
    }
}
