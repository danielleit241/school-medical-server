var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddSignalR();

var app = builder.Build();

await app.AddWebApplicationServicesAsync();

app.Run();


