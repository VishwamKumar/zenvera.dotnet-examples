# Clean Architecture options for the gRPC Todo example

| Dimension | Simple service layer | CQRS with custom dispatcher | CQRS with MediatR |
|---|---|---|---|
| Core shape | gRPC service calls an Application service | gRPC service dispatches explicit commands/queries through in-repo dispatcher | gRPC service sends commands/queries through MediatR |
| Complexity | Low | Medium | Medium–high |
| Boilerplate | Lowest; service interface/implementation and DTO mapping | Command/query/request/handler types plus dispatcher registration | Command/query/handler types plus MediatR registration and optional behaviors |
| Additional dependency count | No mediator package | No mediator package; owns custom dispatcher code | Adds MediatR and its version/lifecycle considerations |
| Testability | High when Application service and repositories are isolated | High; handlers test independently and dispatcher can be tested as infrastructure within Application | High; handlers test independently, while pipeline/integration behavior needs separate tests |
| Cross-cutting pipeline support | Explicit decorators/middleware/service composition | Must design and maintain custom pipeline/decorator semantics | Mature behavior pipeline is available, but behaviors must still be implemented and ordered |
| Maintainability | Strong for small cohesive use cases; large services can accumulate responsibilities | Explicit and dependency-light, but custom dispatch reflection/error semantics become team-owned framework code | Familiar to MediatR teams and extensible; indirection and handler sprawl can hinder navigation |
| Team suitability | Small teams or teams new to the architecture | Teams comfortable owning a narrow dispatcher and conventions | Teams already using mediator/pipeline conventions consistently |
| Recommended application size | Small to medium, especially straightforward CRUD/workflows | Medium systems needing command/query separation without mediator dependency | Medium to large systems with many use cases and justified cross-cutting behaviors |
| Main risks | “Service” classes becoming large; inconsistent cross-cutting behavior | Rebuilding a framework, runtime dispatch failures, registration/reflection complexity | Overusing one handler per trivial operation, hidden control flow, dependency churn, unnecessary ceremony |
| Selection guidance | Start here unless command/query separation solves a concrete problem | Choose when CQRS clarity is valuable and owning a small dispatcher is an explicit decision | Choose when mediator pipelines and team familiarity justify the dependency and indirection |

## Important interpretation

None of the three dispatch choices determines whether the architecture is “clean.” Direct dependency injection is a composition mechanism. The relevant test is whether dependencies point inward: API uses Application policy/abstractions, Infrastructure implements inward-facing persistence contracts, and Domain stays independent. CQRS and mediator patterns may improve organization or cross-cutting composition, but they do not repair incorrect dependency direction by themselves.
