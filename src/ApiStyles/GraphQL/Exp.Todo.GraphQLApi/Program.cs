var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();

builder.Services.AddScoped<ITodoService, ToDoService>();
builder.Services.AddScoped<Query>();
builder.Services.AddScoped<Mutation>();

builder.Services.AddGraphQLServer()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddType<ToDoRequestType>()
        .AddType<ToDoResponseType>()
        .AddFiltering()
        .AddSorting()
        .ModifyPagingOptions(options =>
        {
            options.MaxPageSize = 100;
            options.DefaultPageSize = 10;
            options.IncludeTotalCount = true;
        });


//builder.Services.AddGraphQLServer()
//        .AddQueryType<Query>()
//        .AddMutationType<Mutation>()   
//        .AddType<ToDoRequestType>()
//        .AddType<ToDoResponseType>()
//        .AddFiltering()
//        .AddSorting()
//        .SetPagingOptions(new PagingOptions
//            {
//                MaxPageSize = 100,
//                DefaultPageSize = 10,
//                IncludeTotalCount = true
//            });

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGraphQL();

app.Run();
