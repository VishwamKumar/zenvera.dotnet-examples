var builder = DistributedApplication.CreateBuilder(args);

// Default container lifetime is Session (ephemeral): SQL is created for the AppHost run
// and disposed when it stops. Prefer that for clean starts and orchestration tests.
// For intentional local retention / faster SQL restarts, chain:
//   .WithLifetime(ContainerLifetime.Persistent)
// and document the volume in the example README.
var sql = builder.AddSqlServer("sql");

var database = sql.AddDatabase("reference-db", "ZenveraReference");

// Probe /alive (process liveness). DB readiness is already gated by WaitFor(database);
// /health can stay Unhealthy while EF checks warm up and would block the portal forever.
var api = builder.AddProject<Projects.Zenvera_DotNetExamples_Reference_RestApiHost>("reference-api")
    .WithReference(database)
    .WaitFor(database)
    .WithHttpHealthCheck("/alive");

builder.AddProject<Projects.Zenvera_DotNetExamples_Reference_BlazorServerHost>("reference-portal")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/alive")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
