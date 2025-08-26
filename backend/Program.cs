using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Connection Strings
var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");
var appDbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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

app.Run();