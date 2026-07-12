# SOAP API style example

`Exp.Todo.SoapApi` demonstrates a focused, layered Todo SOAP service implemented with CoreWCF. Its data contracts, service contract, and endpoint behavior are preserved.

## Run and ports

```powershell
dotnet run --project src/ApiStyles/Soap/Exp.Todo.SoapApi
```

- HTTP: `http://localhost:5401`
- HTTPS: `https://localhost:7401`
- Use a SOAP client and the service metadata described under the project's `Docs` folder.

## Dependencies and architecture level

The `net9.0` Web host uses CoreWCF HTTP, AutoMapper, and `shared/Todo/Exp.Todo.Infrastructure.Sqlite`. SQLite creates a local `todo.db`; no external service is required. This is a focused layered interoperability example, not a full Clean Architecture reference.

## Intentional simplifications and production considerations

- WS-Security, authentication, advanced bindings, quotas, versioning policy, and automated contract tests are omitted.
- Production services should review metadata exposure, serializer compatibility, transport security, fault contracts, timeouts, quotas, observability, and client interoperability.

## Comparison summary

SOAP/CoreWCF is the contract-heavy interoperability option. It suits WSDL-oriented and legacy integrations, while REST, gRPC, and GraphQL offer different trade-offs for newer consumers.
