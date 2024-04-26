using System.Text.Json.Serialization;
using server_a.Helpers;
using server_a.HostedService;
using server_a.RealTime;

var builder = WebApplication.CreateBuilder(args);

// dependency injection, register services and functionality
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });
    
builder.Services.AddSingleton<OrdersCollection>();
builder.Services.AddSingleton<SandwichCollection>();

builder.Services.AddCors(option =>{
    option.AddDefaultPolicy(policyBuilder => {
        policyBuilder.WithOrigins("http://localhost:5173", "http://localhost:12346")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });

});

builder.Services.AddSignalR();

builder.Services.AddHostedService<OrderStatusUpdater>();
builder.Services.AddSingleton(MqConnectionCreator.CreateMqConnection(builder.Configuration));

var app = builder.Build();

// middleware, configure the HTTP request pipeline

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
app.MapHub<OrderStatusHub>("/v1/orderStatus");

app.Run();

