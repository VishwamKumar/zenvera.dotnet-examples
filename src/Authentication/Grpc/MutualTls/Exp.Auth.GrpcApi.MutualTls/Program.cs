
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
    {
        options.EnableDetailedErrors = true;
    });
      
builder.Services.AddGrpcReflection(); //Helps Postman to find services
builder.Services.AddLogging();

// Configure Kestrel and HTTPS with mTLS
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var httpsEndpoint = builder.Configuration.GetSection("Kestrel:Endpoints:Https");
    var certName = httpsEndpoint["Certificate:Path"]??"NA";
    var certPass = httpsEndpoint["Certificate:Password"]??"NA";
    Uri httpsUri = new(httpsEndpoint["Url"] ?? "localhost:5001");
    int svcPort = httpsUri.Port;

    serverOptions.ListenAnyIP(svcPort, listenOptions =>
    {
        listenOptions.UseHttps(httpsOptions =>
        {
            try
            {
                // Load the server certificate
                var serverCertificate = new X509Certificate2(certName, certPass);
                httpsOptions.ServerCertificate = serverCertificate;
                httpsOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                httpsOptions.ClientCertificateValidation = (certificate, chain, errors) =>
                {
                    // Custom certificate validation logic here
                    return true; // or implement your own validation logic
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Certificate loading failed.", ex);
            }
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseMiddleware<ExceptionMiddleware>();
app.MapGrpcService<WeatherForecastService>();
app.MapGrpcReflectionService();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
