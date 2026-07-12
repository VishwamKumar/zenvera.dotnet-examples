var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddScoped<ITodoService, ToDoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

builder.Services
   .AddFastEndpoints()
   .SwaggerDocument(); //define a swagger document

var app = builder.Build();

//Connect fast endpoints here
app.UseFastEndpoints()
    .UseSwaggerGen(); 
app.Run();


