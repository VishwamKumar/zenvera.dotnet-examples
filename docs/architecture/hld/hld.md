# High-level design

## Goals

The repository optimizes for comparison, independent execution, honest validation status, and preservation of each example's educational boundary. It does not represent a single deployment topology or unified business domain.

## Organization

- `src/ApiStyles` compares delivery protocols and endpoint organization.
- `src/Authentication` groups mechanisms by protocol.
- `src/Architecture` contains full architecture references.
- `src/Infrastructure` groups focused integrations by capability.
- `src/Integration` preserves cross-service boundaries such as gateways.
- `src/UserInterface` contains the UI frameworks represented by the examples.
- `shared` contains only proven reusable example code.
- `deploy/local` supplies optional local dependencies, not a deployment of every example.
- `docs` contains the catalog, comparisons, decisions, learning paths, and runbooks.

## Build and configuration strategy

One authoritative root `.slnx` inventories all maintained projects. Independently openable category `.slnx` files provide smaller learning and build surfaces without adding project-level solution files. Root build properties set safe deterministic and analyzer defaults without globally changing nullable, implicit usings, language versions, or warnings-as-errors. Package versions remain project-local unless compatibility is proven.

## Runtime model

Each host owns a distinct development port. A learner starts one example and only its required dependencies. Shared SQLite examples use local files; Compose supports Redis, RabbitMQ, MongoDB, and SQL Server; cloud identity, secrets, and gateway downstreams remain external.

## Quality model

Level 3 variants carry automated tests. Level 1 and 2 examples prioritize reproducible manual scenarios unless a test materially improves the lesson. Build prerequisites and production gaps are documented rather than hidden by rewriting examples.

## Key risks

- The MAUI example requires platform-specific workloads and compatible build hosts.
- Infrastructure examples using `Zenvera.Shared.*` require authenticated read access to the configured GitHub Packages feed.
- Development credentials and certificate setup can be mistaken for production guidance without reading the security warnings.
- Over-sharing or over-layering would weaken comparison clarity.

See the [ADR](../adr/ADR-001-reference-example-monorepo.md) and [example catalog](../../catalog.md).
