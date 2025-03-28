using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Data;
using SearchService.Modes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

try {
    await DbInitializer.InitDb(app);
} catch (Exception ex) {
    Console.WriteLine(ex.Message);
}

app.Run();
