using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace server_b;

public class SandwichMaker(ILogger<SandwichMaker> logger, ConnectionFactory rabbitFactory) 
    : IHostedService
{
    private readonly ILogger<SandwichMaker> _logger = logger;
    private EventingBasicConsumer? _consumer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var connection = rabbitFactory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare("orders", false, false, false, null);

        _consumer = new EventingBasicConsumer(channel);
        
        _consumer.Received += OnOrderReceived;
        channel.BasicConsume("orders", true, _consumer);
        
        return Task.CompletedTask;
    }

    private void OnOrderReceived(object? model, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        _logger.LogInformation("Received message: {message}", message);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer?.Model.Dispose();
        return Task.CompletedTask;
    }
}
