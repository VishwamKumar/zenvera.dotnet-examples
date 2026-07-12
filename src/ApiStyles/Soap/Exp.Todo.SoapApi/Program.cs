
var builder = WebApplication.CreateBuilder();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

builder.Services.AddScoped<ITodoService, ToDoService>();
builder.Services.AddScoped<ToDoCoreWcfService>(); //DO NOT ADD INTERFACE HERE, IT WON'T WORK FOR Core WCF SERVICE
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
builder.Services.AddLogging();


var app = builder.Build();
app.UseRouting();


app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<ToDoCoreWcfService>(serviceOptions =>
    {
        serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
    });

    serviceBuilder.AddServiceEndpoint<ToDoCoreWcfService, IToDoCoreWcfService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/ToDoCoreWcfService.svc");
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;

});


app.Run();
