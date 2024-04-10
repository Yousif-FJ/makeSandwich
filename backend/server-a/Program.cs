using RabbitMQ.Client;
using server_a.BackgroundWorkers;
using server_a.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<OrdersCollection>();

var rabbitMqHost = builder.Configuration.GetValue<string>("RabbitMQ_Host");
Console.WriteLine($"RabbitMQ Host: {rabbitMqHost}");


builder.Services.AddHostedService<OrderStatusUpdater>();
builder.Services.AddSingleton(new ConnectionFactory() { HostName = rabbitMqHost });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty;
});
app.UseSwaggerUI();


app.MapControllers();
app.MapSwagger();

app.Run();

