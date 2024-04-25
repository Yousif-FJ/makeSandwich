using RabbitMQ.Client;
using server_b;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<OrderProcessor>();

var rabbitMqHost = builder.Configuration.GetValue<string>("RabbitMQ_Host");
var rabbitMqConnection = new ConnectionFactory() { HostName = rabbitMqHost }.CreateConnection();
builder.Services.AddSingleton(rabbitMqConnection);


var host = builder.Build();
host.Run();
