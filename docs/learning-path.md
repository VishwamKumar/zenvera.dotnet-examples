# Learning path

Use the examples comparatively: run the earlier implementation, identify its boundary and trade-offs, then move to the next model. Paths and status are in the [catalog](catalog.md).

1. **REST implementation styles** — begin with Minimal APIs, then compare MVC controllers, endpoint-per-file organization, and FastEndpoints. Keep routes and behavior in view while comparing ceremony and extension points.
2. **gRPC native and transcoding** — inspect the native Protobuf contract, call the RPC, then compare the annotated transcoding contract and HTTP/JSON surface.
3. **GraphQL and SOAP comparisons** — contrast selectable GraphQL queries with SOAP’s explicit service/data contracts and metadata-oriented interoperability.
4. **Authentication styles** — start with API keys and JWT, then examine Identity/OAuth2, Firebase, and mutual TLS. Separate authentication from authorization and credential lifecycle.
5. **Infrastructure concerns** — add Redis caching, RabbitMQ messaging, secret retrieval, structured logging, and background work one concern at a time.
6. **API gateway** — compare Ocelot and YARP route configuration while keeping downstream ownership and missing resilience/security policy visible.
7. **Simple Clean Architecture** — trace dependency direction through Domain, Application, Infrastructure, gRPC host, and tests without adding a mediator.
8. **CQRS without MediatR** — compare explicit commands/queries and a custom dispatcher with the simple service layer.
9. **CQRS with MediatR** — evaluate library-provided dispatch and pipeline behaviors against added dependency and abstraction cost.
10. **UI framework comparisons** — consume the Todo REST API from the available .NET MAUI client; the catalog currently contains no invented web UI alternatives.

## Suggested study loop

For each example:

1. Read its objective and intentional omissions.
2. Build the smallest relevant category solution.
3. Follow one request or message end to end.
4. Run the documented happy-path and failure scenarios.
5. Compare its production considerations with the next example.
6. Record which complexity is essential for your context and which is teaching scaffolding.
