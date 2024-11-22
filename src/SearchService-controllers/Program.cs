using MongoDB.Driver;
using MongoDB.Entities;
using SearchService_controllers.Data;
using SearchService_controllers.Models;
using SearchService_controllers.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHttpClient<AuctionServiceHttpClient>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

try {
    await DbInitializer.InitDb(app);
} catch (System.Exception ex) {
    Console.WriteLine(ex.Message);
}

app.Run();
