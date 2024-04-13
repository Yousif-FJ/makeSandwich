using RabbitMQ.Client;
using server_b;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<OrderProcessor>();

var rabbitMqHost = builder.Configuration.GetValue<string>("RabbitMQ_Host");

builder.Services.AddSingleton(new ConnectionFactory() { HostName = rabbitMqHost });


var host = builder.Build();
host.Run();
