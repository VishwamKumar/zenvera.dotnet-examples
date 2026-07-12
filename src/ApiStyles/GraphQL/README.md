# GraphQL API style example

`Exp.Todo.GraphQLApi` demonstrates a focused, layered Todo GraphQL API using Hot Chocolate queries and mutations.

## Run and ports

```powershell
dotnet run --project src/ApiStyles/GraphQL/Exp.Todo.GraphQLApi
```

- HTTP: `http://localhost:5301/graphql`
- HTTPS: `https://localhost:7301/graphql`
- The launch profile opens the GraphQL IDE.

## Dependencies and architecture level

The `net9.0` host uses Hot Chocolate ASP.NET Core and EF integration, AutoMapper, and `shared/Todo/Exp.Todo.Infrastructure.Sqlite`. SQLite creates a local `todo.db`; no external service is required. This is a focused layered example, not a full Clean Architecture reference.

## Intentional simplifications and production considerations

- Authentication, pagination, persisted queries, complexity limits, DataLoader strategy, subscriptions, and integration tests are omitted.
- Exception handling and database lifecycle are educational.
- Production systems should address N+1 behavior, query cost/depth, schema compatibility, observability, security, data scoping, and migrations.

## Comparison summary

GraphQL gives clients a schema-driven selectable result shape behind one endpoint. Compared with REST, gRPC, and SOAP, it shifts more shape control to each query and requires explicit resolver-performance and query-cost governance.
