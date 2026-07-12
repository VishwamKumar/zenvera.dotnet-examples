# Example catalog

This catalog lists runnable examples, not every supporting project. `shared/Todo/Exp.Todo.Infrastructure.Sqlite` is a shared Level 2 data component used by API-style hosts and is therefore not listed as an independently runnable example.

Maturity levels are defined in the [root README](../README.md#example-maturity-levels). “Learning only” means the implementation requires a production review and hardening; maturity is not a readiness score.

## API styles

| Category | Example | Level | Project path | Protocol/framework | Domain | External dependencies | Build | Tests | Production readiness | Sequence |
|---|---|---|---|---|---|---|---|---|---|---:|
| REST | Minimal API | Level 2 | `src/ApiStyles/Rest/Exp.Todo.RestApi.Minimal` | HTTP/JSON, ASP.NET Core Minimal APIs | Todo | SQLite file | Pass | Manual | Learning only | 1 |
| REST | MVC controllers | Level 2 | `src/ApiStyles/Rest/Exp.Todo.RestApi.MvcControllers` | HTTP/JSON, ASP.NET Core MVC | Todo | SQLite file | Pass | Manual | Learning only | 2 |
| REST | Endpoint per file | Level 2 | `src/ApiStyles/Rest/Exp.Todo.RestApi.EndpointPerFile` | HTTP/JSON, route handlers | Todo | SQLite file | Pass | Manual | Learning only | 3 |
| REST | FastEndpoints | Level 2 | `src/ApiStyles/Rest/Exp.Todo.RestApi.FastEndpoints` | HTTP/JSON, FastEndpoints | Todo | SQLite file | Pass | Manual | Learning only | 4 |
| gRPC | Native | Level 2 | `src/ApiStyles/Grpc/Exp.Todo.GrpcApi.Native` | HTTP/2, Protobuf, gRPC | Todo | SQLite file | Pass | Manual | Learning only | 5 |
| gRPC | JSON transcoding | Level 2 | `src/ApiStyles/Grpc/Exp.Todo.GrpcApi.Transcoding` | gRPC plus HTTP/JSON | Todo | SQLite file | Pass | Manual | Learning only | 6 |
| GraphQL | Hot Chocolate | Level 2 | `src/ApiStyles/GraphQL/Exp.Todo.GraphQLApi` | GraphQL, Hot Chocolate | Todo | SQLite file | Pass | Manual | Learning only | 7 |
| SOAP | CoreWCF | Level 2 | `src/ApiStyles/Soap/Exp.Todo.SoapApi` | SOAP/WSDL, CoreWCF | Todo | SQLite file | Pass | Manual | Learning only | 8 |

## Authentication

| Category | Example | Level | Project path | Protocol/framework | Domain | External dependencies | Build | Tests | Production readiness | Sequence |
|---|---|---|---|---|---|---|---|---|---|---:|
| REST auth | API key | Level 1 | `src/Authentication/Rest/ApiKey/Exp.Auth.RestApi.ApiKey` | REST middleware | Weather | Local placeholder | Pass | Manual security flow | Internal only after hardening | 9 |
| REST auth | Basic authentication | Level 1 | `src/Authentication/Rest/BasicAuthentication/Exp.Auth.RestApi.BasicAuthentication` | HTTP Basic | Weather | Local placeholder | Pass | Manual security flow | Not recommended for new systems | 10 |
| REST auth | JWT bearer | Level 1 | `src/Authentication/Rest/JwtBearer/Exp.Auth.RestApi.JwtBearer` | REST, symmetric JWT | Weather | Local placeholder | Pass | Manual security flow | Learning issuer; hardening required | 11 |
| REST auth | JWT with Identity | Level 1 | `src/Authentication/Rest/JwtBearerIdentity/Exp.Auth.RestApi.JwtBearerIdentity` | ASP.NET Identity, JWT | Weather/auth | SQL Server | Pass | Manual security flow | Viable only after full hardening | 12 |
| REST auth | OAuth2 with Duende | Level 1 | `src/Authentication/Rest/OAuth2Duende/Exp.Auth.RestApi.OAuth2Duende` | OAuth2/OIDC, Duende | Weather/auth | Local HTTPS; licensing review | Pass | Manual security flow | Viable only after full hardening | 13 |
| gRPC auth | API key | Level 1 | `src/Authentication/Grpc/ApiKey/Exp.Auth.GrpcApi.ApiKey` | gRPC metadata | Weather | Local placeholder | Pass | Manual security flow | Internal only after hardening | 14 |
| gRPC auth | JWT bearer | Level 1 | `src/Authentication/Grpc/JwtBearer/Exp.Auth.GrpcApi.JwtBearer` | gRPC metadata plus JWT | Weather | Local placeholder | Pass | Manual security flow | Learning issuer; hardening required | 15 |
| gRPC auth | Mutual TLS | Level 1 | `src/Authentication/Grpc/MutualTls/Exp.Auth.GrpcApi.MutualTls` | gRPC, client certificates | Weather | Local PKI/certificates | Pass | Manual TLS flow | Viable with managed PKI | 16 |
| GraphQL auth | API key | Level 1 | `src/Authentication/GraphQL/ApiKey/Exp.Auth.GraphQLApi.ApiKey` | GraphQL HTTP middleware | Weather | Local placeholder | Pass | Manual security flow | Internal only after hardening | 17 |
| GraphQL auth | JWT bearer | Level 1 | `src/Authentication/GraphQL/JwtBearer/Exp.Auth.GraphQLApi.JwtBearer` | Hot Chocolate authorization, JWT | Weather | Local placeholder | Pass | Manual security flow | Learning issuer; hardening required | 18 |
| GraphQL auth | Firebase | Level 1 | `src/Authentication/GraphQL/FirebaseAuthentication/Exp.Auth.GraphQLApi.FirebaseAuthentication` | Firebase ID token, GraphQL | Weather | Firebase project and credentials | Pass build; external runtime | Manual security flow | Viable with managed credentials | 19 |

## Infrastructure, integration, architecture, and UI

| Category | Example | Level | Project path | Protocol/framework | Domain | External dependencies | Build | Tests | Production readiness | Sequence |
|---|---|---|---|---|---|---|---|---|---|---:|
| Infrastructure | Redis caching | Level 1 | `src/Infrastructure/Caching/Redis/Exp.Weather.RestApi.RedisCaching` | REST, Redis | Weather | Redis; private GitHub package feed | Requires authenticated restore | Manual | Learning only | 20 |
| Infrastructure | RabbitMQ publishing | Level 1 | `src/Infrastructure/Messaging/RabbitMq/Exp.Weather.RestApi.RabbitMq` | REST, AMQP | Weather | RabbitMQ; private GitHub package feed | Requires authenticated restore | Manual | Learning only | 21 |
| Infrastructure | Azure Key Vault | Level 1 | `src/Infrastructure/Secrets/AzureKeyVault/Exp.Weather.BlazorServer.KeyVault` | Blazor Server, Key Vault | Weather/secrets | Azure vault; private GitHub package feed | Requires authenticated restore | Manual | Learning only | 22 |
| Infrastructure | REST Serilog | Level 1 | `src/Infrastructure/Logging/Serilog/Exp.Weather.RestApi.Serilog` | REST, structured logging | Weather | SQL Server; private GitHub package feed | Requires authenticated restore | Manual | Learning only | 23 |
| Infrastructure | Blazor Serilog | Level 1 | `src/Infrastructure/Logging/Serilog/Exp.Weather.BlazorServer.Serilog` | Blazor Server, MongoDB logging | Weather | MongoDB; private GitHub package feed | Requires authenticated restore | Manual | Learning only | 24 |
| Infrastructure | Background service | Level 1 | `src/Infrastructure/BackgroundProcessing/BackgroundService/Exp.Weather.Worker.BackgroundService` | ASP.NET hosted service | Weather | None | Pass | Manual | Learning only | 25 |
| Integration | Ocelot gateway | Level 1 | `src/Integration/ApiGateway/RestApis/Ocelot/Exp.ApiGateway.Ocelot` | REST reverse proxy, Ocelot | Gateway | External downstream APIs | Pass build; external runtime | Manual | Learning only | 26 |
| Integration | YARP gateway | Level 1 | `src/Integration/ApiGateway/RestApis/Yarp/Exp.ApiGateway.Yarp` | REST reverse proxy, YARP | Gateway | Hour Tracker API | Pass build; external runtime | Manual | Learning only | 27 |
| Clean Architecture | Simple service layer | Level 3 | `src/Architecture/CleanArchitecture/GrpcTodo/Simple` | gRPC, EF Core, service layer | Todo | SQLite file | Pass | 27 automated | Reference, still needs hardening | 28 |
| Clean Architecture | Custom CQRS | Level 3 | `src/Architecture/CleanArchitecture/GrpcTodo/Cqrs` | gRPC, custom dispatcher | Todo | SQLite file | Pass | 5 automated | Reference, still needs hardening | 29 |
| Clean Architecture | CQRS with MediatR | Level 3 | `src/Architecture/CleanArchitecture/GrpcTodo/CqrsMediatR` | gRPC, MediatR pipelines | Todo | SQLite file | Pass with `NU1510` | 5 automated | Reference, still needs hardening | 30 |
| User interface | .NET MAUI REST client | Level 1 | `src/UserInterface/Maui/Exp.Todo.Maui.RestApiClient` | .NET MAUI 10 native UI, REST | Todo | Todo REST API; platform workloads and compatible hosts | Restored; Android assembly compiled locally; multi-target CI excluded | Manual | Learning only | 31 |

## Status interpretation

Build status reflects the .NET 10 migration validation with SDK 10.0.301. A passing build does not prove runtime dependencies or security configuration are present. Host- or credential-dependent validation remains explicit rather than hidden.
