using EventServiceProvider.Data;
using EventServiceProvider.Handlers;
using EventServiceProvider.Repositories;
using EventServiceProvider.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventDb")));

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMemoryCache();

builder.Services.AddScoped(typeof(ICacheHandler<>), typeof(CacheHandler<>));

builder.Services.AddScoped<IEventRepository, EventRepository>();

//builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<EventService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
