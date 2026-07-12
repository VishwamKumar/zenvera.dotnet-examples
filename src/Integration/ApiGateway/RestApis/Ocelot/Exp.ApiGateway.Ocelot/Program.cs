var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
//.AddJsonFile("gatewaySettings.json", optional: false, reloadOnChange: true)

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)    
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)   
    .AddJsonFile("gatewaySettings.json", true,true)
    .AddJsonFile($"gatewaySettings.{builder.Environment.EnvironmentName}.json")    
    //.AddJsonFile($"configuration.{builder.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables();

//builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddOcelot(builder.Configuration).AddCacheManager(settings=>settings.WithDictionaryHandle());
builder.Services.AddSwaggerForOcelot(builder.Configuration,
  (o) =>
  {
      o.GenerateDocsForGatewayItSelf = true;         
  });

// Add services to the container.
 //builder.Services.AddMvc();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vishwam Rest Gateway Service", Version = "v1", Description = "Vishwam Rest Gateway Service v1" });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";        
    });

//app.UseSwaggerForOcelotUI(opt =>
//{
//    opt.DownstreamSwaggerHeaders = new[]
//    {
//      new KeyValuePair<string, string>("X-API-Key", "KeyValue"),
//    };
//});

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.Map("/swagger/v1/swagger.json", b =>
{
    b.Run(async x => {
        var json = File.ReadAllText("swagger.json");
        await x.Response.WriteAsync(json);
    });
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ocelot");
});

app.UseOcelot().Wait();

//app.UseRouting();
//app.MapControllers();
//await app.UseOcelot();

app.Run();
