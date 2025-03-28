using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app) {
            await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

            await DB.Index<Item>()
                .Key(i => i.Make, KeyType.Text)
                .Key(i => i.Model, KeyType.Text)
                .Key(i => i.Color, KeyType.Text)
                .CreateAsync();

            var count = await DB.CountAsync<Item>();
            
            using var scope = app.Services.CreateScope();

            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();
            var items = await httpClient.GetItemsFromSearchDb();

            Console.WriteLine($"Items count: {items.Count}");

            if (items.Count > 0) await DB.SaveAsync(items);
        }
    }
}