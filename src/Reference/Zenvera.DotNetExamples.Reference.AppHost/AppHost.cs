var builder = DistributedApplication.CreateBuilder(args);

// Default container lifetime is Session (ephemeral): SQL is created for the AppHost run
// and disposed when it stops. Prefer that for clean starts and orchestration tests.
// For intentional local retention / faster SQL restarts, chain:
//   .WithLifetime(ContainerLifetime.Persistent)
// and document the volume in the example README.
var sql = builder.AddSqlServer("sql");

var database = sql.AddDatabase("reference-db", "ZenveraReference");

var api = builder.AddProject<Projects.Zenvera_DotNetExamples_Reference_RestApiHost>("reference-api")
    .WithReference(database)
    .WaitFor(database)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Zenvera_DotNetExamples_Reference_BlazorServerHost>("reference-portal")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
