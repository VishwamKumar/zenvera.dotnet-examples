var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddHttpClient();
builder.Services.AddControllers();
//Following needed for Background Service
builder.Services.AddHostedService<WeatherBackgroundService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure logging to Console
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
