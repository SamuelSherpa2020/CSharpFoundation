# Eric Evans – Domain-Driven Design Study Guide

A plain-language guide to Eric Evans's *Domain-Driven Design: Tackling Complexity in the Heart of Software* (2003), with examples in **C# / .NET 8+**, EF Core, React, and Azure.

| # | File | Covers |
|---|---|---|
| 01 | [01-Domain-Driven-Design.md](01-Domain-Driven-Design.md) | The whole book: Ubiquitous Language, Entities, Value Objects, Aggregates, Repositories, Domain Events, Specification, Supple Design, Bounded Contexts, Context Maps, Core Domain, plus a full .NET example, Azure mapping, a planning checklist, and interview Q&A |

## How it connects to the Robert C. Martin guides

- **[Clean Architecture](../Rober%20C%20Martin/03-Clean-Architecture.md)** tells you *where* the domain lives (at the center, depending on nothing). **DDD** tells you *how to model* it and *how to split* a large system into Bounded Contexts.
- **[Agile PPP](../Rober%20C%20Martin/05-Agile-Software-Development-PPP.md)** gives you SOLID and patterns (Strategy, Specification-like predicates, Composite) that DDD uses inside the model.
- **[Functional Design](../Rober%20C%20Martin/07-Functional-Design.md)**: immutable value objects, pure domain functions, and event sourcing are where DDD and FP meet.
- **[Clean Agile](../Rober%20C%20Martin/04-Clean-Agile.md)**: the "Metaphor" practice is the ancestor of the Ubiquitous Language.

## One-page cheat sheet

- **Strategic first:** Core Domain → Bounded Contexts → Context Map.
- **Language:** one Ubiquitous Language per context, used in the code.
- **Model:** a rich model, not an anemic one. Behavior lives with the data.
- **Entity** = identity · **Value Object** = immutable value · **Aggregate** = consistency boundary with one root.
- **Small aggregates**, reference others by ID, one aggregate per transaction, domain events for the rest.
- **Repository** per aggregate root; read models for queries.
- **Anticorruption Layer** around anything foreign or legacy.
- **Invest in the core, buy the generic.**
