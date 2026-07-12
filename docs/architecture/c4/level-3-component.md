# C4 level 3 — representative components

This component view describes recurring repository roles without claiming every Level 1 example contains every component.

```mermaid
flowchart TB
  subgraph Focused[Level 1 focused example]
    Host1[Protocol/UI host]
    Concern[Mechanism integration]
    Host1 --> Concern
  end
  subgraph Layered[Level 2 layered example]
    Delivery[REST / gRPC / GraphQL / SOAP delivery]
    Mapping[DTO and mapping layer]
    SharedData[Shared Todo service and EF Core SQLite]
    Delivery --> Mapping --> SharedData
  end
  subgraph Reference[Level 3 architecture reference]
    Grpc[gRPC host / composition root]
    Application[Application services or CQRS handlers]
    Domain[Domain entities and rules]
    Infrastructure[EF Core repository implementations]
    Tests[Unit and integration tests]
    Grpc --> Application
    Grpc --> Infrastructure
    Infrastructure --> Application
    Application --> Domain
    Infrastructure --> Domain
    Tests --> Application
    Tests --> Infrastructure
  end
```

Dependency direction—not the use or absence of dependency injection or MediatR—is the relevant architectural test in the Level 3 examples.
