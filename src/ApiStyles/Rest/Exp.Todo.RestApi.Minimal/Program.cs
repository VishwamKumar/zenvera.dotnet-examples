var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddScoped<ITodoService, ToDoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

string apiPref = "api/todos"; // Base API prefix


// GET: api/todos
app.MapGet($"{apiPref}", async (ITodoService todoService, IMapper mapper) =>
{
    var items = await todoService.GetToDosAsync();
    var itemsDto = mapper.Map<List<ToDoResponse>>(items);
    return Results.Ok(itemsDto);
});

// GET: api/todos/{id}
app.MapGet($"{apiPref}/{{id}}", async (ITodoService todoService, IMapper mapper, [FromRoute] int id) =>
{
    var toDo = await todoService.GetToDoByIdAsync(id);
    if (toDo == null)
    {
        return Results.NotFound();
    }
    var toDoDto = mapper.Map<ToDoResponse>(toDo);
    return Results.Ok(toDoDto);
});

// POST: api/todos
app.MapPost($"{apiPref}", async (ITodoService todoService, ILogger<Program> logger, IMapper mapper, [FromBody] ToDoRequest req) =>
{
    try
    {
        ToDo toDo = new() { ToDoName = req.ToDoName };
        var toDoDto = await todoService.AddToDoAsync(toDo);
        if (toDoDto != null)
        {
            return Results.Created($"/{apiPref}/{toDo.Id}", toDoDto);
        }
        else
        {
            return Results.BadRequest(new { message = "The ToDo item could not be created. Please check the input and try again." });
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, ex?.Message);
        throw;
    }
});

// PUT: api/todos/{id}
app.MapPut($"{apiPref}/{{id}}", async (ITodoService todoService, IMapper mapper, ILogger<Program> logger, [FromRoute] int id, [FromBody] ToDoRequest req) =>
{
    try
    {
        var toDo = await todoService.GetToDoByIdAsync(id);
        if (toDo == null)
        {
            return Results.NotFound();
        }
        toDo.ToDoName = req.ToDoName;
        var rec = await todoService.UpdateToDoAsync(toDo);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, ex.Message);
        throw;
    }    
    return Results.NoContent();
});

// DELETE: api/todos/{id}
app.MapDelete($"{apiPref}/{{id}}", async (ITodoService todoService, ILogger<Program> logger, [FromRoute] int id) =>
{
    try
    {
        var toDo = await todoService.GetToDoByIdAsync(id);
        if (toDo == null)
        {
            return Results.NotFound();
        }
        _ = await todoService.DeleteToDoByIdAsync(id);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, ex.Message);
        throw;
    }
    return Results.NoContent();
});

app.Run();


