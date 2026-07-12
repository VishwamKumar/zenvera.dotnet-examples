# C4 level 2 — repository containers

“Container” here means a major repository/runtime grouping. A learner normally selects one branch, not the entire graph.

```mermaid
flowchart LR
  Learner[Developer / learner]
  Solution[Root .slnx and category filters]
  Api[API-style hosts]
  Auth[Authentication hosts]
  Arch[Clean Architecture variants]
  Infra[Infrastructure pattern hosts]
  Gateway[Gateway hosts]
  UI[MAUI client]
  Shared[Shared Todo persistence]
  Local[(Local Compose services)]
  External[External dev services/APIs]

  Learner --> Solution
  Solution --> Api
  Solution --> Auth
  Solution --> Arch
  Solution --> Infra
  Solution --> Gateway
  Solution --> UI
  Api --> Shared
  UI --> Api
  Infra --> Local
  Infra --> External
  Gateway --> External
  Auth --> External
```

The root solution is an inventory/build surface. Category filters are the preferred focused build surfaces. Shared code is deliberately narrow; authentication and infrastructure implementations remain local to their demonstrations.
