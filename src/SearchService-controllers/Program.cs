using MongoDB.Driver;
using MongoDB.Entities;
using SearchService_controllers.Data;
using SearchService_controllers.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

try {
    await DbInitializer.InitDb(app);
} catch (System.Exception ex) {
    Console.WriteLine(ex.Message);
}

app.Run();
