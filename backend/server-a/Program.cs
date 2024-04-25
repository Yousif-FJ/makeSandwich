using System.Text.Json.Serialization;
using RabbitMQ.Client;
using server_a.Helpers;
using server_a.HostedService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });
    
builder.Services.AddSingleton<OrdersCollection>();

builder.Services.AddCors(option =>{
    option.AddDefaultPolicy(policyBuilder => {
        policyBuilder.WithOrigins("http://localhost:5173", "http://localhost:12346")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

});


builder.Services.AddHostedService<OrderStatusUpdater>();
builder.Services.AddSingleton(MqConnectionCreator.CreateMqConnection(builder.Configuration));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty;
});
app.UseSwaggerUI();

app.UseCors();

app.MapControllers();
app.MapSwagger();

app.Run();

