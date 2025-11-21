using InventoryManagementSystem.Models;
using InventoryManagementSystem.Hubs;
using InventoryManagementSystem.BackgroundServices;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<StockDbSettings>(builder.Configuration.GetSection("ProduitDB"));

builder.Services.AddControllers().AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null); ;
builder.Services.AddSignalR();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHostedService<NotificationSendService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(_ => true); // allow localhost clients
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseCors();

app.MapHub<NotificationHub>("/hubs/Notification");
app.Run();
