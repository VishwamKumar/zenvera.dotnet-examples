# C4 level 1 — repository context

The subject is the example catalog repository, not a single deployed business system.

```mermaid
C4Context
  title Zenvera .NET Examples — learning context
  Person(learner, "Learner or contributor", "Builds, runs, compares, and extends examples")
  System(repo, "zenvera.dotnet-examples", "Reference-example monorepo containing independently runnable .NET examples")
  System_Ext(localInfra, "Local infrastructure", "Redis, RabbitMQ, SQL Server, MongoDB")
  System_Ext(cloud, "External development services", "Azure Key Vault, Firebase, downstream APIs")
  System_Ext(tooling, ".NET and container tooling", "SDK, IDE, protocol clients, Docker Compose")
  Rel(learner, repo, "Reads, builds, runs, tests")
  Rel(repo, localInfra, "Selected examples connect to")
  Rel(repo, cloud, "Selected examples authenticate to or proxy")
  Rel(learner, tooling, "Uses")
```

Each example has its own runtime boundary. The diagram shows shared learning context only; it does not imply that all examples deploy together.
