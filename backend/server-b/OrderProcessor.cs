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
    private IModel? _mqChannel;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _mqChannel = mqConnection.CreateModel();
        _mqChannel.QueueDeclare("orders", false, false, false, null);

        var consumer = new EventingBasicConsumer(_mqChannel);
        consumer.Received += OnOrderReceived;

        _mqChannel.BasicConsume("orders", false, consumer);


        _mqChannel.ExchangeDeclare("orderStatusUpdates", ExchangeType.Direct);
        _mqChannel.QueueDeclare("orderStatusUpdates", false, false, false, null);
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
            order.Status = StatusEnum.Received;
            
            SendOrderToOrderStatusQueue(order);
            _mqChannel!.BasicAck(ea.DeliveryTag, false);            


            Task.Delay(10000).GetAwaiter().GetResult();

            order.Status = StatusEnum.Ready;
            SendOrderToOrderStatusQueue(order);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error processing order. Message was sent to dead letter queue.");

            _mqChannel!.QueueDeclare("deadLetter", false, false, false, null);
            _mqChannel!.BasicPublish("", "deadLetter", null, ea.Body);
            _mqChannel!.BasicAck(ea.DeliveryTag, false);           
        }
    }

    private void SendOrderToOrderStatusQueue(Order order)
    {
        var statusMessage = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order));
        _mqChannel.BasicPublish("orderStatusUpdates", "orderStatus", null, statusMessage);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _mqChannel?.Dispose();
        return Task.CompletedTask;
    }
}
