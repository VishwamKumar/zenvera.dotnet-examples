
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddGrpc()
                .AddJsonTranscoding();
builder.Services.AddGrpcReflection(); //Helps Postman to find services
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

builder.Services.AddScoped<ITodoService, ToDoService>();
builder.Services.AddLogging();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.MapGrpcService<DoerService>();
app.MapGrpcReflectionService();

app.MapGet("/", () => "Hello, Communication with gRPC endpoints must be made through a gRPC client.");

//Alternate option if launchsettingss are not adjusted for launchurl
//app.MapGet("/", () =>
//{
//    // Redirect to Swagger UI
//    return Results.Redirect("/swagger");
//});


app.Run();
