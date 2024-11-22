using MongoDB.Driver;
using MongoDB.Entities;
using SearchService_controllers.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

await DB.InitAsync("SearchDb", MongoClientSettings
    .FromConnectionString(builder.Configuration.GetConnectionString("MongoDbConnection")));

await DB.Index<Item>()
    .Key(a => a.Make, KeyType.Text)
    .Key(a => a.Model, KeyType.Text)
    .Key(a => a.Color, KeyType.Text)
    .CreateAsync();

app.Run();
