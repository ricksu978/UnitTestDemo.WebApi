using UnitTestDemo.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddScoped<EchoHub>();

var app = builder.Build();

app.MapControllers();
app.MapHub<EchoHub>("/socket");

app.Run();
