using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMassTransit(x => {
    x.AddEntityFrameworkOutbox<AuctionDbContext>(o => {
        o.QueryDelay = TimeSpan.FromSeconds(10);
        o.UsePostgres();
        o.UseBusOutbox();
    });
    x.UsingRabbitMq((context, cfg) => {
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.TokenValidationParameters.ValidateAudience = false;        
        options.TokenValidationParameters.NameClaimType = "username";
        options.RequireHttpsMetadata = false; // Set to true in production
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try {
    DbInitializer.InitDb(app);
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        DbInitializer.SeedData(dbContext);
    }
} catch (Exception ex)
{
    Console.WriteLine($"Error initializing database: {ex.Message}");
}

app.Run();
