# UI framework comparison

The migrated source contains one UI framework, so this is an inventory rather than a synthetic multi-framework benchmark.

| Framework/model | Example | State and backend | Benefits | Constraints | Recommended use |
|---|---|---|---|---|---|
| .NET MAUI native, Shell navigation | `Exp.Todo.Maui.RestApiClient` | Page-local state backed by a Todo REST API; no authentication | Shared C#/XAML code with native platform controls | Requires platform SDK workloads, device-aware API addressing, and lifecycle/offline design | Cross-platform native companion and line-of-business clients |

No Blazor, MVC, Razor Pages, or WebAssembly example existed in `exp.ui-styles.frameworks`; empty categories and invented implementations were not added.
