using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService_controllers.Data;
using SearchService_controllers.Models;
using SearchService_controllers.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHttpClient<AuctionServiceHttpClient>()
    .AddPolicyHandler(GetPolicy());

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () => {
    try {
    await DbInitializer.InitDb(app);
    } catch (System.Exception ex) {
        Console.WriteLine(ex.Message);
    }
});



app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy() {
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
