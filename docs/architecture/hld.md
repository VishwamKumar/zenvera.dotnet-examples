# High-level design

## Goals

The repository optimizes for comparison, independent execution, honest validation status, and preservation of each example’s educational boundary. It does not optimize for a single deployment topology or unified business domain.

## Organization

- `src/ApiStyles` compares delivery protocols and endpoint organization.
- `src/Authentication` groups mechanisms by protocol.
- `src/Architecture` contains full architecture references.
- `src/Infrastructure` groups focused integrations by capability.
- `src/Integration` preserves cross-service boundaries such as gateways.
- `src/UserInterface` contains actual UI frameworks present in the sources.
- `shared` contains only proven reusable example code.
- `deploy/local` supplies optional local dependencies, not a deployment of every example.
- `docs` contains catalog, comparisons, decisions, learning paths, and runbooks.

## Build and configuration strategy

One root `.slnx` inventories all maintained projects. Solution filters provide smaller learning/build surfaces. Root build properties set safe deterministic/analyzer defaults without globally changing nullable, implicit usings, language versions, or warnings-as-errors. Package versions remain project-local unless compatibility is proven.

## Runtime model

Each host owns a distinct development port. A learner starts one example and only its required dependencies. Shared SQLite examples use local files; Compose supports Redis, RabbitMQ, MongoDB, and SQL Server; cloud identity/secrets and gateway downstreams stay external.

## Quality model

Level 3 variants carry automated tests. Level 1 and 2 examples prioritize reproducible manual scenarios unless a test materially improves the lesson. Build blockers and production gaps are cataloged rather than hidden by rewriting examples.

## Key risks

- Framework diversity requires multiple targeting packs/workloads.
- Some migrated `Zenvera.Shared.*` package versions are unavailable.
- Development credentials and certificate setup can be mistaken for production guidance without reading security warnings.
- Over-sharing or over-layering would weaken comparison clarity.

See the [ADR](adr/ADR-001-reference-example-monorepo.md), [catalog](../catalog.md), and [consistency report](../repository-consistency-report.md).
