using backend.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection Strings
var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");
var appDbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(appDbConnectionString));

// Services
builder.Services.AddScoped<IPlantService, PlantService>();

// Hangfire Services
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(hangfireConnectionString)); // Database Connection for hangfire

// Hangfire Server
builder.Services.AddHangfireServer();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Hangfire Dashboard
app.UseHangfireDashboard();

app.MapControllers();

// Recurring Job Manager
var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate<IPlantService>(
    "plant-watering", // Unique Id
    service => service.WaterAllPlants(),
    Cron.Daily); // "It is an abbreviation for ‘0 0 * * *’. It runs every midnight..

recurringJobManager.AddOrUpdate<IPlantService>(
    "plant-sunlight",
    service => service.GiveSunlightToAllPlants(),
    "0 */4 * * *"); // "every 4 hours"

recurringJobManager.AddOrUpdate<IPlantService>(
    "plant-health-check",
    service => service.PerformHealthCheckAsync(),
    Cron.Hourly); // "0 * * * *"

app.Run();