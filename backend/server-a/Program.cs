using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server_a.Data.Collections;
using server_a.Data.Database;
using server_a.Helpers;
using server_a.HostedService;
using server_a.RealTime;

var builder = WebApplication.CreateBuilder(args);

// dependency injection, register services and functionality
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>{
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddControllers().AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });
    

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(DbStringCreator.CreateDbString(builder.Configuration)));

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
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddHostedService<OrderStatusUpdater>();
builder.Services.AddHostedService<SeedIdentity>();
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

app.UseAuthorization();

app.MapControllers();
app.MapSwagger();
app.MapHub<OrderStatusHub>("/v1/orderStatus");

app.Run();

