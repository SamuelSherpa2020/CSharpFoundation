# Clean Architecture: A Craftsman's Guide to Software Structure and Design

> **Author:** Robert C. Martin ("Uncle Bob") · **Published:** 2017
> **One-line summary:** Keep your **business rules** at the center, independent of frameworks, databases, and UI. All source-code dependencies point **inward**, toward the business rules.
> **Examples in this guide:** C# 12 / .NET 8+, ASP.NET Core, EF Core, React, Azure.

---

## Table of Contents

1. [What the Book Actually Covers](#1-what-the-book-actually-covers)
2. [Core Teachings with C# Examples](#2-core-teachings-with-c-examples)
   - 2.1 [Why Architecture Matters – The Two Values](#21-why-architecture-matters--the-two-values)
   - 2.2 [Programming Paradigms](#22-programming-paradigms)
   - 2.3 [SOLID Principles](#23-solid-principles)
   - 2.4 [Component Principles](#24-component-principles)
   - 2.5 [What Architecture Is](#25-what-architecture-is)
   - 2.6 [Boundaries – Drawing Lines](#26-boundaries--drawing-lines)
   - 2.7 [Policy and Level](#27-policy-and-level)
   - 2.8 [Business Rules – Entities & Use Cases](#28-business-rules--entities--use-cases)
   - 2.9 [Screaming Architecture](#29-screaming-architecture)
   - 2.10 [The Clean Architecture & the Dependency Rule](#210-the-clean-architecture--the-dependency-rule)
   - 2.11 [Presenters and Humble Objects](#211-presenters-and-humble-objects)
   - 2.12 [Partial Boundaries](#212-partial-boundaries)
   - 2.13 [The Main Component](#213-the-main-component)
   - 2.14 [Services: Great and Small](#214-services-great-and-small)
   - 2.15 [The Test Boundary](#215-the-test-boundary)
   - 2.16 [Details: Database, Web, Frameworks](#216-details-database-web-frameworks)
   - 2.17 [The Missing Chapter – Packaging Options](#217-the-missing-chapter--packaging-options)
3. [Full Worked Example – A .NET Clean Architecture Solution](#3-full-worked-example--a-net-clean-architecture-solution)
4. [Mapping to React and Azure](#4-mapping-to-react-and-azure)
5. [Planning Checklist – Before You Build](#5-planning-checklist--before-you-build)
6. [Interview Questions & Answers](#6-interview-questions--answers)
7. [Modern Nuance – Don't Over-Engineer](#7-modern-nuance--dont-over-engineer)
8. [Key Takeaways](#8-key-takeaways)

---

## 1. What the Book Actually Covers

| Part | Topic | Big idea |
|---|---|---|
| **I. Introduction** | What design and architecture are | They're the same thing. Goal: **minimize the human effort to build and maintain the system.** |
| **II. Programming Paradigms** | Structured, Object-Oriented, Functional | Each paradigm *removes* a capability (goto, function pointers, assignment) to impose discipline. |
| **III. Design Principles** | SOLID | How to arrange functions and data into classes, and how classes connect. |
| **IV. Component Principles** | REP, CCP, CRP, ADP, SDP, SAP | How to group classes into deployable components (assemblies/projects) and how components depend on each other. |
| **V. Architecture** | Boundaries, policies, business rules, the Dependency Rule, Clean Architecture diagram, humble objects, services, testing | The core of the book. |
| **VI. Details** | The database, web, and frameworks are **details** | Keep them at the edges; don't let them dictate your design. |
| **Missing Chapter** (Simon Brown) | Package by layer / feature / ports & adapters / component | How to actually organize code so the compiler enforces the architecture. |

**Central message:**

> The goal of software architecture is to **minimize the lifetime cost** of the system. A good architecture makes the system easy to **understand, develop, maintain, and deploy**, and it **keeps options open** by deferring decisions about details (DB, framework, UI) as long as possible.

---

## 2. Core Teachings with C# Examples

### 2.1 Why Architecture Matters – The Two Values

Software gives stakeholders two values:

1. **Behavior** — it does what they need today (features).
2. **Structure (architecture)** — it is *soft*: easy to change tomorrow.

**Uncle Bob's argument:** Structure is the **more important** of the two.
- A program that works perfectly but is impossible to change becomes useless when requirements change (and they always do).
- A program that doesn't work but is easy to change can be made to work.

**The Eisenhower Matrix applied:**

| | Urgent | Not Urgent |
|---|---|---|
| **Important** | 1. Urgent & important | **2. Architecture** (important, rarely urgent) |
| **Not important** | 3. Many features (urgent, not important) | 4. — |

Business tends to push #3 above #2. **It's the developers' job to fight for architecture.** Nobody else will.

**Real-world example:** A startup builds an MVP with all logic in controllers and EF entities. 18 months later, adding a simple "pay by invoice" option takes 6 weeks and breaks three other things. Each release gets slower. That's the cost of ignoring structure.

---

### 2.2 Programming Paradigms

| Paradigm | What it takes away | Why it matters for architecture |
|---|---|---|
| **Structured** | Direct transfer of control (`goto`) | Lets us decompose programs into provable small units → **functional decomposition**, testability. |
| **Object-Oriented** | Indirect transfer of control (raw function pointers) | Safe **polymorphism** → lets us **invert dependencies** (plugin architecture). *This is OO's real superpower.* |
| **Functional** | Assignment (mutable state) | **Immutability** removes race conditions, concurrency problems. Segregate mutable and immutable parts. |

#### OO's power = dependency inversion through polymorphism

```csharp
// Without polymorphism: business logic depends on the database. Flow of control and
// source-code dependency point the same way.
public class ReportService
{
    private readonly SqlServerReportStore _store = new();   // 🔗 hard dependency on a detail
}

// With polymorphism: the business owns the interface; the DB implements it.
// Source-code dependency is INVERTED relative to the flow of control.
public interface IReportStore { Task<Report> LoadAsync(ReportId id); }   // in Domain/Application
public class ReportService(IReportStore store) { /* ... */ }             // depends only on abstraction
public class SqlReportStore : IReportStore { /* ... */ }                // in Infrastructure, depends INWARD
```

#### Functional: immutability in C#

```csharp
public sealed record Money(decimal Amount, string Currency)
{
    public Money Add(Money other) =>
        other.Currency == Currency
            ? this with { Amount = Amount + other.Amount }
            : throw new InvalidOperationException("Currency mismatch");
}
```

**Event sourcing** (mentioned in the book) is the functional idea at scale: store immutable events, derive state.

---

### 2.3 SOLID Principles

SOLID tells us how to arrange functions and data into classes and how to connect them. Goal: structures that **tolerate change**, are **easy to understand**, and are **reusable**.

#### S — Single Responsibility Principle

> **A module should be responsible to one, and only one, actor.**

Note: It's *not* "a class should do one thing." It's about **who requests changes**.

**Real-world example:** An `Employee` class with:
- `CalculatePay()` → used by **Accounting (CFO)**
- `ReportHours()` → used by **HR (COO)**
- `Save()` → used by **DBAs (CTO)**

If accounting changes the overtime rule and `CalculatePay` and `ReportHours` share a helper `RegularHours()`, HR's report silently breaks. Three actors, one class = accidental coupling.

```csharp
// ❌ Three actors, one class
public class Employee
{
    public decimal CalculatePay() { /* uses RegularHours() */ }
    public string ReportHours()   { /* uses RegularHours() */ }
    public void Save()            { /* SQL */ }
    private decimal RegularHours() { /* shared – danger! */ }
}

// ✅ Separate per actor; data shared via a plain data structure
public sealed record EmployeeData(Guid Id, string Name, decimal HourlyRate, IReadOnlyList<TimeCard> TimeCards);

public sealed class PayCalculator   { public decimal CalculatePay(EmployeeData e) { ... } }   // CFO
public sealed class HourReporter    { public string ReportHours(EmployeeData e) { ... } }    // COO
public sealed class EmployeeSaver   { public Task SaveAsync(EmployeeData e) { ... } }        // CTO

// Optional Facade if callers want one entry point
public sealed class EmployeeFacade(PayCalculator pay, HourReporter hours, EmployeeSaver saver) { ... }
```

#### O — Open/Closed Principle

> **A software artifact should be open for extension but closed for modification.**

At architecture level: arrange components so that **higher-level components are protected from changes in lower-level ones.**

**Real-world example:** A financial report shown on web; now the business wants it as PDF too. Good architecture → you *add* a PDF presenter, you don't touch the report calculation.

```csharp
public interface IReportPresenter { Task PresentAsync(FinancialReport report); }

public sealed class FinancialReportGenerator(IFinancialDataGateway gateway)
{
    public FinancialReport Generate(Period period) { /* business rules – never changes for new formats */ }
}

public sealed class WebReportPresenter : IReportPresenter { ... }   // existing
public sealed class PdfReportPresenter : IReportPresenter { ... }   // NEW — added, nothing modified
```

#### L — Liskov Substitution Principle

> **Subtypes must be substitutable for their base types** without the caller knowing.

**Classic violation:** `Square : Rectangle` (setting width changes height — breaks callers' expectations).

**Architectural violation (from the book):** A taxi-dispatch system with REST URIs. One company (`Acme`) abbreviates `destination` as `dest`. Now the dispatch code gets `if (company == "Acme")` hacks everywhere. Fix: put the differences in a **configuration-driven adapter**, so every company is substitutable.

```csharp
// ❌ LSP broken – caller must know the subtype
public decimal CalculateShipping(IShippingProvider provider, Parcel p)
{
    if (provider is FreeShippingProvider) return 0;       // type check = LSP smell
    return provider.Quote(p);
}

// ✅ All providers honor the same contract
public interface IShippingProvider { decimal Quote(Parcel parcel); }
public sealed class FreeShippingProvider : IShippingProvider { public decimal Quote(Parcel _) => 0m; }
```

> **.NET gotcha:** `ReadOnlyCollection<T>` implements `IList<T>` but throws on `Add` — a well-known LSP violation in the BCL. Prefer `IReadOnlyList<T>` in your own APIs.

#### I — Interface Segregation Principle

> **Don't depend on things you don't use.**

```csharp
// ❌ Fat interface: a reporting service now depends on write methods it never calls.
//    Any change to Add/Delete forces recompiling/redeploying the reporting component.
public interface IOrderRepository
{
    Task<Order?> GetAsync(Guid id);
    Task<IReadOnlyList<Order>> SearchAsync(OrderFilter f);
    Task AddAsync(Order o);
    Task DeleteAsync(Guid id);
    Task<SalesStats> GetStatsAsync(DateRange r);
}

// ✅ Role interfaces
public interface IOrderReader  { Task<Order?> GetAsync(Guid id); Task<IReadOnlyList<Order>> SearchAsync(OrderFilter f); }
public interface IOrderWriter  { Task AddAsync(Order o); Task DeleteAsync(Guid id); }
public interface ISalesStats   { Task<SalesStats> GetStatsAsync(DateRange r); }
```

At architecture level: don't depend on a huge framework when you only need a tiny part of it — you inherit all its baggage.

#### D — Dependency Inversion Principle

> **High-level policy should not depend on low-level details. Both should depend on abstractions.** Source-code dependencies should refer only to abstractions, not concretions.

Rules of thumb:
- Don't refer to volatile concrete classes. Refer to interfaces.
- Don't derive from volatile concrete classes.
- Don't override concrete functions.
- Never mention the name of anything concrete and volatile (use factories / DI).

Stable concrete things (`string`, `List<T>`, `DateTime`… the .NET BCL) are fine to depend on.

```csharp
// Domain/Application owns the abstraction:
public interface IEmailSender { Task SendAsync(EmailMessage message, CancellationToken ct); }

// Infrastructure provides the detail:
public sealed class AzureCommunicationEmailSender(EmailClient client) : IEmailSender { ... }

// Composition root wires them (the only place that knows concretes):
builder.Services.AddSingleton<IEmailSender, AzureCommunicationEmailSender>();
```

---

### 2.4 Component Principles

**Components** = units of deployment. In .NET: **projects / assemblies / NuGet packages** (and at a higher level, deployable services).

#### Component Cohesion — what goes *into* a component

| Principle | Meaning | .NET example |
|---|---|---|
| **REP** – Reuse/Release Equivalence | The unit of reuse is the unit of release. Things released together should be versioned together and make sense as a group. | A `Contoso.Payments` NuGet package with semantic versioning and release notes. |
| **CCP** – Common Closure | Gather classes that **change for the same reasons at the same times**. (SRP for components.) | All "Invoicing" rules in `Invoicing.Domain`, not split across 4 projects. |
| **CRP** – Common Reuse | Don't force users to depend on things they don't need. (ISP for components.) | Don't put `PdfGenerator` in `Shared.Core` that every project references. |

These three pull against each other — the **tension diagram**:

```
              REP (group for reusers)
               /\
              /  \
   too many  /    \  too many
   unneeded /      \ components
   releases/        \ change
          /          \
   CCP --------------- CRP
   (group for maintenance)   (split to avoid unneeded releases)
```

Early in a project, favor **CCP** (developability). As it matures and others reuse it, shift toward **REP/CRP**.

#### Component Coupling — relationships *between* components

**ADP – Acyclic Dependencies Principle:** No cycles in the component dependency graph.

```
❌ Orders → Customers → Billing → Orders      (cycle: you can't build/test/release any of them independently)
✅ Break it with DIP: Billing defines IOrderLookup, Orders implements it.
```

.NET helps you here — project references *can't* be circular. But cycles still sneak in via shared "Common" projects or between microservices calling each other.

**SDP – Stable Dependencies Principle:** Depend in the direction of stability.

- **Instability I = Fan-out / (Fan-in + Fan-out)**
  - Fan-in = number of components that depend on this one
  - Fan-out = number of components this one depends on
- I = 0 → maximally stable (many depend on it, it depends on nothing — hard to change)
- I = 1 → maximally unstable (depends on others, nothing depends on it — easy to change)
- **Rule:** I of a component should be *larger* than the I of components it depends on.

**SAP – Stable Abstractions Principle:** A component should be as abstract as it is stable.

- **Abstractness A = (abstract classes + interfaces) / total classes**
- Stable components should be abstract (so they can be extended). Unstable components should be concrete.
- **Main sequence:** ideal components lie on the line A + I = 1.
  - **Zone of Pain** (A≈0, I≈0): concrete and stable — e.g., a DB schema everyone depends on. Painful to change.
  - **Zone of Uselessness** (A≈1, I≈1): abstract and nobody uses it — dead interfaces.

#### Quick C# helper to compute these metrics

```csharp
public sealed record ComponentMetrics(string Name, int FanIn, int FanOut, int AbstractTypes, int TotalTypes)
{
    public double Instability  => FanIn + FanOut == 0 ? 0 : (double)FanOut / (FanIn + FanOut);
    public double Abstractness => TotalTypes == 0 ? 0 : (double)AbstractTypes / TotalTypes;
    public double DistanceFromMainSequence => Math.Abs(Abstractness + Instability - 1);
}

// Domain: FanIn=3 (Application, Infrastructure, Api), FanOut=0 → I = 0 (stable). Should be fairly abstract.
// Api:    FanIn=0, FanOut=3 → I = 1 (unstable). Concrete – fine.
```

(Tools like **NDepend** compute this automatically for .NET solutions.)

---

### 2.5 What Architecture Is

> **The architecture of a system is the shape given to it by those who build it** — the division into components, their arrangement, and how they communicate.

**Purpose:** support the **lifecycle** of the system:
- **Development** — teams can work independently.
- **Deployment** — ideally a single action.
- **Operation** — makes the system's purpose obvious to operators.
- **Maintenance** — the most costly phase; minimize "spelunking" and risk.

**Keep options open.** Every system has two parts:
- **Policy** — business rules and procedures. *The real value.*
- **Details** — DB, web server, frameworks, protocols, UI. *Necessary, but irrelevant to the policy.*

> **A good architect maximizes the number of decisions not made.**

**Real-world example:** Uncle Bob's FitNesse project deferred choosing a database. They used flat files behind an interface. Eventually they found they never needed a database at all. Decision deferred → work avoided.

**For you:** You can build and test the whole order-processing logic before deciding **Azure SQL vs Cosmos DB**. Start with an in-memory repository; decide later with real data on access patterns.

#### Independence

A good architecture supports:
- **Use cases** — the architecture should *scream* what the system does.
- **Operation** — scale, throughput (should allow switching from monolith → services if needed).
- **Development** — Conway's law: teams map to components.
- **Deployment** — independent deployability.

**Decoupling modes** (from least to most isolated):
1. **Source level** — one assembly, separate namespaces/classes.
2. **Deployment level** — separate DLLs / packages, same process.
3. **Service level** — separate processes communicating over the network.

> Start with source/deployment-level decoupling; push to services **only when needed**. A well-decoupled monolith can be split later; a tangled one can't.

**True vs. accidental duplication:** Two pieces of code look the same but change for different reasons (e.g., an API DTO and a DB entity with the same fields). That's *accidental* duplication — **don't merge them**. Merging couples things that should evolve independently.

---

### 2.6 Boundaries – Drawing Lines

> **Boundaries separate software elements from one another and restrict those on one side from knowing about those on the other.**

Draw lines between **things that matter** (business rules) and **things that don't** (GUI, DB, frameworks).

**Plugin architecture:** The DB and the GUI are *plugins* to the business rules. Like how a Visual Studio extension plugs into Visual Studio — VS doesn't depend on the extension.

```
          +------------------+
   GUI ──▶|                  |◀── Database
          | Business Rules   |
  Email ─▶|                  |◀── Payment Provider
          +------------------+
  All arrows point INTO the business rules.
```

**Boundary anatomy (from least to most heavy):**
- **Monolith:** boundary = interfaces + DI within one deployable.
- **Deployment components:** separate DLLs/NuGet packages.
- **Threads:** not a boundary, just a way to organize execution.
- **Local processes:** separate executables on the same machine.
- **Services:** separate processes over the network — strongest boundary, highest cost.

---

### 2.7 Policy and Level

- Software is a collection of **policies**.
- **Level** = distance from inputs and outputs. The farther from I/O, the **higher** the level.
- **Higher-level policies change less often and for more important reasons.**
- Source-code dependencies should point **toward higher-level policies**.

**Example (encryption program from the book):**
```
Read chars → [Encrypt (HIGH level)] → Write chars
```
`Encrypt` should not depend on `ConsoleReader` or `FileWriter`. It should depend on `ICharReader` / `ICharWriter`, which the low-level I/O classes implement.

```csharp
public interface ICharReader { int Read(); }      // owned by high-level policy
public interface ICharWriter { void Write(char c); }

public sealed class Encryptor(ICharReader reader, ICharWriter writer, ICipher cipher)
{
    public void Run()
    {
        int c;
        while ((c = reader.Read()) != -1)
            writer.Write(cipher.Translate((char)c));
    }
}
// ConsoleCharReader, BlobCharReader, FileCharWriter… are low-level plugins.
```

---

### 2.8 Business Rules – Entities & Use Cases

#### Entities (Enterprise / Critical Business Rules)

> Rules that would exist **even if there were no computer.** They make or save the business money.

Example: A bank charges N% interest on a loan. That rule existed when bankers used paper ledgers.

```csharp
// Domain/Loans/Loan.cs – pure C#, no EF attributes, no ASP.NET, no JSON attributes
public sealed class Loan
{
    public LoanId Id { get; }
    public Money Principal { get; private set; }
    public decimal AnnualInterestRate { get; }
    public DateOnly StartDate { get; }

    public Loan(LoanId id, Money principal, decimal annualInterestRate, DateOnly startDate)
    {
        if (principal.Amount <= 0) throw new DomainException("Principal must be positive.");
        if (annualInterestRate is < 0 or > 1) throw new DomainException("Rate must be between 0 and 1.");
        (Id, Principal, AnnualInterestRate, StartDate) = (id, principal, annualInterestRate, startDate);
    }

    public Money MonthlyPayment(int termInMonths)
    {
        var r = AnnualInterestRate / 12;
        if (r == 0) return Principal with { Amount = Principal.Amount / termInMonths };
        var factor = (decimal)Math.Pow((double)(1 + r), termInMonths);
        return Principal with { Amount = Math.Round(Principal.Amount * r * factor / (factor - 1), 2) };
    }

    public void ApplyPayment(Money payment)
    {
        if (payment.Amount > Principal.Amount) throw new DomainException("Overpayment not allowed.");
        Principal = Principal with { Amount = Principal.Amount - payment.Amount };
    }
}
```

#### Use Cases (Application-Specific Business Rules)

> Rules that exist **because the software is automated.** They describe *how* the system is used — inputs, outputs, steps — and orchestrate entities.

Example: "Create a loan application: validate contact info, check credit score ≥ 500, then create the loan."

- Use cases **don't know** about the UI or DB — only about interfaces.
- Use cases take **simple request models** and return **simple response models** — not entities, not `HttpRequest`.

```csharp
// Application/Loans/CreateLoan/CreateLoanRequest.cs
public sealed record CreateLoanRequest(Guid CustomerId, decimal Amount, string Currency, int TermMonths);
public sealed record CreateLoanResponse(Guid LoanId, decimal MonthlyPayment);

// Application/Loans/CreateLoan/CreateLoanUseCase.cs
public sealed class CreateLoanUseCase(
    ICustomerGateway customers,
    ICreditScoreService credit,
    ILoanRepository loans,
    IInterestRatePolicy ratePolicy,
    IClock clock)
{
    private const int MinimumCreditScore = 500;

    public async Task<CreateLoanResponse> ExecuteAsync(CreateLoanRequest request, CancellationToken ct)
    {
        var customer = await customers.GetAsync(request.CustomerId, ct)
                       ?? throw new NotFoundException("Customer", request.CustomerId);

        var score = await credit.GetScoreAsync(customer.Id, ct);
        if (score < MinimumCreditScore)
            throw new LoanRejectedException($"Credit score {score} below {MinimumCreditScore}.");

        var loan = new Loan(
            LoanId.New(),
            new Money(request.Amount, request.Currency),
            ratePolicy.RateFor(score),
            DateOnly.FromDateTime(clock.UtcNow));

        await loans.AddAsync(loan, ct);

        return new CreateLoanResponse(loan.Id.Value, loan.MonthlyPayment(request.TermMonths).Amount);
    }
}
```

Notice: **no** `DbContext`, **no** `HttpContext`, **no** `[JsonProperty]`. Just business.

---

### 2.9 Screaming Architecture

> **Your architecture should scream the use cases of the system**, not the framework.

Look at the top-level folders of a building blueprint: "Kitchen, Bedroom, Living Room" — it *screams* "house." Your solution should scream "**Healthcare system**" or "**Loan management**", not "**ASP.NET MVC**."

#### ❌ Screams "framework"

```
src/
  Controllers/
  Models/
  Views/
  Services/
  Repositories/
  Helpers/
```

#### ✅ Screams "business"

```
src/
  Loans/
    CreateLoan/
    ApplyPayment/
    CloseLoan/
  Customers/
    RegisterCustomer/
    UpdateContactInfo/
  CreditChecks/
  Statements/
```

> **Frameworks are tools, not ways of life.** Look at a framework with a jaundiced eye: how will it constrain you? How will you get out if needed?

---

### 2.10 The Clean Architecture & the Dependency Rule

This is the famous concentric-circles diagram (combining Hexagonal / Ports & Adapters, Onion, DCI, BCE):

```
 ┌──────────────────────────────────────────────────────────────┐
 │  Frameworks & Drivers   (Web, UI, DB, Devices, External APIs)│
 │  ┌────────────────────────────────────────────────────────┐  │
 │  │  Interface Adapters  (Controllers, Presenters, Gateways)│  │
 │  │  ┌──────────────────────────────────────────────────┐  │  │
 │  │  │  Application Business Rules  (Use Cases)         │  │  │
 │  │  │  ┌────────────────────────────────────────────┐  │  │  │
 │  │  │  │  Enterprise Business Rules  (Entities)     │  │  │  │
 │  │  │  └────────────────────────────────────────────┘  │  │  │
 │  │  └──────────────────────────────────────────────────┘  │  │
 │  └────────────────────────────────────────────────────────┘  │
 └──────────────────────────────────────────────────────────────┘
               ALL SOURCE-CODE DEPENDENCIES POINT INWARD ──▶
```

### ⭐ The Dependency Rule

> **Source-code dependencies must point only inward, toward higher-level policies.** Nothing in an inner circle can know anything about something in an outer circle — no names of classes, functions, variables, or data formats.

**Layer by layer:**

| Circle | Contains | .NET project | May reference |
|---|---|---|---|
| **Entities** | Enterprise business rules | `Shop.Domain` | Nothing (only BCL) |
| **Use Cases** | Application business rules, ports (interfaces) | `Shop.Application` | Domain |
| **Interface Adapters** | Controllers, presenters, view models, repository implementations, mappers | `Shop.Infrastructure`, `Shop.Api` | Application, Domain |
| **Frameworks & Drivers** | ASP.NET Core, EF Core, Azure SDKs, React | (NuGet packages, the React app) | Everything |

**Crossing boundaries — how does a use case call the DB if it can't reference it?**
**Dependency inversion.** The use case defines an *output port* interface (`ILoanRepository`); the outer layer implements it. Flow of control goes outward; source-code dependency points inward.

```
Controller ──calls──▶ UseCase ──calls──▶ ILoanRepository  (interface, defined in Application)
                                                ▲
                                                │ implements
                                         EfLoanRepository  (Infrastructure)
```

**What data crosses boundaries?** Simple data structures (DTOs, records). **Never** pass EF entities or `HttpRequest` objects inward. Data passed across a boundary should be in the form most convenient for the **inner** circle.

#### A typical scenario — the full flow

```
HTTP POST /loans
   → LoansController (adapter) maps JSON → CreateLoanRequest
     → CreateLoanUseCase (application)
         → Loan entity (domain)  — business rules
         → ILoanRepository (port)  ← EfLoanRepository (adapter) → Azure SQL (driver)
     ← CreateLoanResponse
   ← Presenter/Controller maps → 201 Created + JSON
```

---

### 2.11 Presenters and Humble Objects

**Humble Object pattern:** Split hard-to-test behavior from easy-to-test behavior.
- The **humble** part is so simple it barely needs testing (just moves data to the screen / DB).
- The **testable** part contains all the logic.

**Presenter / View:**
- **Presenter** (testable): formats data into a **View Model** — strings, booleans, flags. E.g., dates formatted, money formatted, "should the Delete button be disabled?"
- **View** (humble): dumb — binds the View Model to the screen.

```csharp
// Testable presenter
public sealed record LoanViewModel(string LoanId, string Principal, string MonthlyPayment, bool CanClose, string StatusCssClass);

public sealed class LoanPresenter(ICurrencyFormatter fmt)
{
    public LoanViewModel Present(LoanDetailsResponse r) => new(
        LoanId: r.Id.ToString("N")[..8].ToUpperInvariant(),
        Principal: fmt.Format(r.Principal, r.Currency),
        MonthlyPayment: fmt.Format(r.MonthlyPayment, r.Currency),
        CanClose: r.Principal == 0,
        StatusCssClass: r.IsOverdue ? "status--danger" : "status--ok");
}

[Fact]
public void PaidOffLoan_CanBeClosed()
{
    var vm = new LoanPresenter(new InvariantCurrencyFormatter())
        .Present(new LoanDetailsResponse(Guid.NewGuid(), 0m, 120m, "EUR", IsOverdue: false));
    Assert.True(vm.CanClose);
}
```

In a React SPA, the API often returns a view-model–shaped DTO and the **React component is the humble view**:

```tsx
// Humble: no logic, just renders what the presenter decided
export function LoanCard({ vm }: { vm: LoanViewModel }) {
  return (
    <div className={vm.statusCssClass}>
      <h3>Loan {vm.loanId}</h3>
      <p>Principal: {vm.principal}</p>
      <button disabled={!vm.canClose}>Close loan</button>
    </div>
  );
}
```

**Other humble objects:** database gateways (the SQL/EF code is humble; the use case is testable), service listeners (the Service Bus trigger is humble; the handler is testable).

---

### 2.12 Partial Boundaries

Full boundaries (interfaces, separate DTOs, separate projects) are expensive. Sometimes you want to *prepare* for a boundary without paying full price (YAGNI vs. foresight):

1. **Skip the last step:** Make everything ready for separate components, but keep them in one assembly.
2. **One-dimensional boundary (Strategy):** Only an interface on one side — the client depends on `IServiceBoundary`.
3. **Facade:** A simple class that forwards calls — cheapest; no dependency inversion.

> Senior judgment: decide *where* a boundary might be needed, estimate the cost of implementing it now vs. later, and **re-evaluate as the system evolves.**

---

### 2.13 The Main Component

> **Main is the dirtiest, lowest-level component.** It creates all the factories, strategies, and global facilities, then hands control to the high-level abstract parts.

In .NET: **`Program.cs`** (the composition root). Think of Main as a **plugin** to the application: you could have a `Program.cs` for Dev, one for Test, one for Prod, each wiring different implementations.

```csharp
// Shop.Api/Program.cs — the "Main" component. Knows everything; nobody knows it.
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDomain()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)   // EF Core, Azure SDK clients
    .AddPresentation();

var app = builder.Build();
app.MapLoanEndpoints();
app.Run();
```

---

### 2.14 Services: Great and Small

**Microservices are not automatically an architecture.**

- **Fallacy of decoupling:** Services can still be coupled by shared data schemas. Add a field to a shared record → every service that uses it must change.
- **Fallacy of independent development & deployment:** If services are coupled by data or behavior, they must be developed and deployed in coordination.
- **The "Kitty problem" (from the book):** A taxi-aggregator split into services. A new feature (kitten delivery!) cuts across *every* service → all must change. Services were split **by function**, not by **change axes**.

**Fix:** Design services internally with SOLID / component architecture (plugins, polymorphism), so new features are added as new components, not changes across all services.

> **The architecture is defined by the boundaries *within* the system and the dependencies that cross them — not by the physical mechanism (process, network) used to communicate.**

**For Azure:** A well-structured **modular monolith** on App Service is often a better first architecture than 12 Container Apps. Split out services when you have a real reason (scale, team autonomy, different deployment cadence).

---

### 2.15 The Test Boundary

- **Tests are part of the system** — the outermost circle. Nothing depends on them; they depend inward.
- **Fragile tests problem:** Tests coupled to the GUI or to implementation details break with every change → teams stop changing code.
- **Design for testability:** Create a **testing API** — a set of hooks that let tests verify business rules without going through the UI or DB.
- **Structural coupling:** Don't have one test class per production class mirroring internal structure. Test **behavior through stable APIs** (use cases), so you can refactor internals freely.

```csharp
// Test the use case directly – fast, no web server, no DB
public class CreateLoanUseCaseTests
{
    [Fact]
    public async Task LowCreditScore_IsRejected()
    {
        var useCase = new CreateLoanUseCase(
            customers: new InMemoryCustomerGateway(Customer.Sample()),
            credit: new FixedCreditScore(420),
            loans: new InMemoryLoanRepository(),
            ratePolicy: new FlatRatePolicy(0.05m),
            clock: new FixedClock(new DateTime(2026, 11, 19)));

        await Assert.ThrowsAsync<LoanRejectedException>(() =>
            useCase.ExecuteAsync(new CreateLoanRequest(Customer.SampleId, 10_000, "EUR", 24), default));
    }
}
```

---

### 2.16 Details: Database, Web, Frameworks

#### The Database is a Detail

- The **data model** is important. The **database** (technology) is a detail.
- Business rules shouldn't care if data is in SQL Server, Cosmos DB, a file, or memory.
- Don't let tables/rows/ORM types leak into business rules.

```csharp
// Domain – no EF attributes, no navigation-property gymnastics
public sealed class Customer { /* ... */ }

// Infrastructure – EF mapping lives HERE, not on the entity
public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> b)
    {
        b.ToTable("Customers");
        b.HasKey(c => c.Id);
        b.Property(c => c.Id).HasConversion(id => id.Value, v => new CustomerId(v));
        b.OwnsOne(c => c.Email, e => e.Property(x => x.Value).HasColumnName("Email").HasMaxLength(256));
    }
}
```

#### The Web is a Detail

The web is just another I/O device. History oscillates (mainframe → PC → web → SPA → server components…). Your business rules shouldn't care whether they're called from a React SPA, a Blazor app, a mobile app, a CLI, or an Azure Function.

#### Frameworks are Details

- **Don't marry the framework.** The author of the framework doesn't know your problem; the framework wants *you* to couple to it.
- **Risks:** framework architecture may not be clean; as your product grows you may outgrow it; framework may evolve in ways you don't like; a better one may appear.
- **Solution:** Keep the framework at arm's length — in the outer circle. Don't derive your entities from framework base classes. Don't put framework attributes on business objects.
- **Exception:** Some "frameworks" are effectively the language's standard library — in .NET, depending on the BCL, `System.*`, LINQ, `Microsoft.Extensions.DependencyInjection.Abstractions` is fine.

```csharp
// ❌ Married to the framework
public class Order : IdentityUser, INotifyPropertyChanged  // business entity derives from framework types
{
    [Required, JsonPropertyName("order_id"), Column("OrderID")]
    public int Id { get; set; }
}

// ✅ Dating the framework
public sealed class Order { public OrderId Id { get; } /* ... */ }   // Domain: pure
public sealed record OrderDto(Guid OrderId, ...);                    // Api: JSON shape
// EF mapping in Infrastructure configuration class
```

---

### 2.17 The Missing Chapter – Packaging Options

(Written by Simon Brown.) Four ways to organize code:

| Approach | Structure | Pros / Cons |
|---|---|---|
| **Package by layer** | `Controllers/`, `Services/`, `Repositories/` | Simple to start; doesn't scream; hard to scale; easy to bypass layers |
| **Package by feature** | `Orders/`, `Customers/` with everything inside | Screams the domain; can still mix concerns inside a folder |
| **Ports & Adapters** | `Domain/` (inside) + `Infrastructure/` (outside) | Strong separation of business vs. tech |
| **Package by component** | One component per business capability, exposing a public interface; internals hidden | Balanced; uses compiler to enforce boundaries |

**Key insight:** *Use the compiler to enforce architecture.* If everything is `public`, your architecture is just a suggestion.

In C#:
- Make implementation classes **`internal`**; expose only interfaces/use cases as `public`.
- Use separate **projects** so references enforce direction.
- Add **architecture tests**:

```csharp
// NetArchTest.Rules (or ArchUnitNET) – the build fails if someone breaks the Dependency Rule
public class ArchitectureTests
{
    [Fact]
    public void Domain_ShouldNotDependOn_OtherLayers()
    {
        var result = Types.InAssembly(typeof(Loan).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny("Shop.Application", "Shop.Infrastructure", "Shop.Api",
                                 "Microsoft.EntityFrameworkCore", "Microsoft.AspNetCore")
            .GetResult();

        Assert.True(result.IsSuccessful, string.Join(", ", result.FailingTypeNames ?? []));
    }

    [Fact]
    public void Application_ShouldNotDependOn_Infrastructure() =>
        Assert.True(Types.InAssembly(typeof(CreateLoanUseCase).Assembly)
            .ShouldNot().HaveDependencyOn("Shop.Infrastructure").GetResult().IsSuccessful);
}
```

---

## 3. Full Worked Example – A .NET Clean Architecture Solution

### Solution layout

```
Shop.sln
│
├── src/
│   ├── Shop.Domain/                    ← Entities (no project references, no NuGet except maybe primitives)
│   │   ├── Orders/
│   │   │   ├── Order.cs
│   │   │   ├── OrderLine.cs
│   │   │   ├── OrderStatus.cs
│   │   │   └── Events/OrderPlaced.cs
│   │   ├── Customers/Customer.cs
│   │   └── Common/{Money.cs, DomainException.cs, Entity.cs}
│   │
│   ├── Shop.Application/               ← Use cases + ports  (refs: Domain)
│   │   ├── Orders/
│   │   │   ├── PlaceOrder/{PlaceOrderCommand.cs, PlaceOrderHandler.cs, PlaceOrderValidator.cs}
│   │   │   └── GetOrder/{GetOrderQuery.cs, GetOrderHandler.cs, OrderDto.cs}
│   │   └── Abstractions/{IOrderRepository.cs, IUnitOfWork.cs, IPaymentGateway.cs, IClock.cs, IEventPublisher.cs}
│   │
│   ├── Shop.Infrastructure/            ← Adapters to details  (refs: Application, Domain)
│   │   ├── Persistence/{ShopDbContext.cs, Configurations/, Repositories/EfOrderRepository.cs}
│   │   ├── Payments/StripePaymentGateway.cs
│   │   ├── Messaging/ServiceBusEventPublisher.cs
│   │   └── DependencyInjection.cs
│   │
│   ├── Shop.Api/                       ← Delivery mechanism + composition root  (refs: Application, Infrastructure)
│   │   ├── Endpoints/OrderEndpoints.cs
│   │   └── Program.cs
│   │
│   └── shop-web/                       ← React + TypeScript SPA (talks to Shop.Api over HTTP)
│
└── tests/
    ├── Shop.Domain.Tests/
    ├── Shop.Application.Tests/
    ├── Shop.Api.IntegrationTests/       (WebApplicationFactory + Testcontainers)
    └── Shop.ArchitectureTests/          (NetArchTest)
```

### Domain — entity with business rules

```csharp
namespace Shop.Domain.Orders;

public sealed class Order : Entity<OrderId>
{
    private readonly List<OrderLine> _lines = [];
    public CustomerId CustomerId { get; }
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;
    public IReadOnlyList<OrderLine> Lines => _lines;
    public Money Total => _lines.Aggregate(Money.Zero("EUR"), (sum, l) => sum.Add(l.Subtotal));

    private Order(OrderId id, CustomerId customerId) : base(id) => CustomerId = customerId;

    public static Order Create(CustomerId customerId) => new(OrderId.New(), customerId);

    public void AddLine(ProductId productId, int quantity, Money unitPrice)
    {
        EnsureStatus(OrderStatus.Draft);
        if (quantity <= 0) throw new DomainException("Quantity must be positive.");
        _lines.Add(new OrderLine(productId, quantity, unitPrice));
    }

    public void Place(DateTime utcNow)
    {
        EnsureStatus(OrderStatus.Draft);
        if (_lines.Count == 0) throw new DomainException("Cannot place an empty order.");
        Status = OrderStatus.Placed;
        Raise(new OrderPlaced(Id, CustomerId, Total, utcNow));
    }

    private void EnsureStatus(OrderStatus expected)
    {
        if (Status != expected) throw new DomainException($"Order must be {expected} but is {Status}.");
    }
}
```

### Application — use case + ports

```csharp
namespace Shop.Application.Abstractions;

public interface IOrderRepository
{
    Task<Order?> GetAsync(OrderId id, CancellationToken ct);
    Task AddAsync(Order order, CancellationToken ct);
}
public interface IUnitOfWork { Task SaveChangesAsync(CancellationToken ct); }
public interface IClock { DateTime UtcNow { get; } }
```

```csharp
namespace Shop.Application.Orders.PlaceOrder;

public sealed record PlaceOrderCommand(Guid CustomerId, IReadOnlyList<PlaceOrderLine> Lines);
public sealed record PlaceOrderLine(Guid ProductId, int Quantity);
public sealed record PlaceOrderResult(Guid OrderId, decimal Total, string Currency);

public sealed class PlaceOrderHandler(
    IOrderRepository orders,
    IProductCatalog catalog,
    IUnitOfWork unitOfWork,
    IClock clock)
{
    public async Task<PlaceOrderResult> Handle(PlaceOrderCommand cmd, CancellationToken ct)
    {
        var order = Order.Create(new CustomerId(cmd.CustomerId));

        foreach (var line in cmd.Lines)
        {
            var price = await catalog.GetPriceAsync(new ProductId(line.ProductId), ct);
            order.AddLine(new ProductId(line.ProductId), line.Quantity, price);
        }

        order.Place(clock.UtcNow);

        await orders.AddAsync(order, ct);
        await unitOfWork.SaveChangesAsync(ct);   // domain events dispatched by infrastructure on save

        return new PlaceOrderResult(order.Id.Value, order.Total.Amount, order.Total.Currency);
    }
}
```

### Infrastructure — adapters

```csharp
namespace Shop.Infrastructure.Persistence.Repositories;

internal sealed class EfOrderRepository(ShopDbContext db) : IOrderRepository
{
    public Task<Order?> GetAsync(OrderId id, CancellationToken ct) =>
        db.Orders.Include(o => o.Lines).FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task AddAsync(Order order, CancellationToken ct) =>
        await db.Orders.AddAsync(order, ct);
}

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ShopDbContext>(o => o.UseSqlServer(config.GetConnectionString("Shop")));
        services.AddScoped<IOrderRepository, EfOrderRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ShopDbContext>());
        services.AddSingleton<IClock, SystemClock>();
        services.AddScoped<IProductCatalog, SqlProductCatalog>();
        return services;
    }
}
```

### API — thin delivery layer

```csharp
namespace Shop.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").RequireAuthorization();

        group.MapPost("/", async (PlaceOrderCommand cmd, PlaceOrderHandler handler, CancellationToken ct) =>
        {
            var result = await handler.Handle(cmd, ct);
            return Results.Created($"/api/orders/{result.OrderId}", result);
        });
    }
}
```

### Unit test — no DB, no web, milliseconds

```csharp
[Fact]
public async Task PlaceOrder_CalculatesTotal()
{
    var repo = new InMemoryOrderRepository();
    var handler = new PlaceOrderHandler(repo, new FakeCatalog(price: 10m), new NoOpUnitOfWork(), new FixedClock());

    var result = await handler.Handle(
        new PlaceOrderCommand(Guid.NewGuid(), [new(Guid.NewGuid(), 3)]), default);

    Assert.Equal(30m, result.Total);
    Assert.Single(repo.Saved);
}
```

**What we gained:**
- Swap SQL Server → Cosmos DB: change only `Shop.Infrastructure`.
- Swap REST → Azure Functions / gRPC: add a new delivery project; use cases untouched.
- 90% of tests run without any infrastructure.
- Folder names scream "Orders", "Customers".

---

## 4. Mapping to React and Azure

### React (the frontend has its own clean architecture)

```
shop-web/src/
  features/
    orders/
      domain/        ← types + pure functions (e.g., calculateCartTotal) – no React, no fetch
      api/           ← adapters: ordersApi.ts (fetch/axios) implementing an interface
      hooks/         ← use cases as hooks: usePlaceOrder()
      components/    ← humble views
  shared/
  app/               ← composition root: providers, router, QueryClient
```

```ts
// domain – pure, testable with Vitest, no React
export const cartTotal = (lines: CartLine[]) => lines.reduce((s, l) => s + l.unitPrice * l.qty, 0);

// port
export interface OrdersGateway { placeOrder(cmd: PlaceOrderCommand): Promise<PlaceOrderResult>; }

// adapter
export const httpOrdersGateway: OrdersGateway = {
  placeOrder: (cmd) => fetch("/api/orders", { method: "POST", body: JSON.stringify(cmd),
    headers: { "Content-Type": "application/json" } }).then(r => r.json()),
};
```

### Azure — infrastructure is a detail too

| Clean Architecture concept | Azure realization |
|---|---|
| Delivery mechanism (outer circle) | Azure App Service / Container Apps / Azure Functions / Static Web Apps (React) |
| Gateway to DB | EF Core → **Azure SQL**; Cosmos SDK → **Cosmos DB** (behind `IOrderRepository`) |
| Event publisher port | **Azure Service Bus** / Event Grid adapter behind `IEventPublisher` |
| File storage port | **Blob Storage** adapter behind `IDocumentStore` |
| Secrets/config | **Key Vault** + **App Configuration** → `IOptions<T>` |
| Observability | **Application Insights** via OpenTelemetry (cross-cutting, outer layer) |
| Main / composition root | `Program.cs` reads environment config and wires Azure adapters |
| Test doubles | Azurite (Storage emulator), Service Bus emulator, Testcontainers for SQL |

**Example: the same use case, two delivery mechanisms**

```csharp
// Delivery 1: HTTP API (App Service)
group.MapPost("/", (PlaceOrderCommand cmd, PlaceOrderHandler h, CancellationToken ct) => h.Handle(cmd, ct));

// Delivery 2: Azure Function triggered by a queue (isolated worker)
public sealed class PlaceOrderFromQueue(PlaceOrderHandler handler)
{
    [Function(nameof(PlaceOrderFromQueue))]
    public Task Run([ServiceBusTrigger("incoming-orders", Connection = "ServiceBus")] PlaceOrderCommand cmd,
                    CancellationToken ct) => handler.Handle(cmd, ct);
}
```

Same business rule, zero duplication. That's the payoff.

---

## 5. Planning Checklist – Before You Build

Use this in your design/kick-off sessions:

**Business first**
- [ ] What are the **core use cases**? (List them as verbs: Place Order, Refund Payment…)
- [ ] What are the **enterprise rules** that exist without software?
- [ ] Who are the **actors**? (Each actor = a reason to change → informs SRP boundaries.)

**Boundaries**
- [ ] Where are the **boundaries** between business rules and details (DB, UI, external APIs, messaging)?
- [ ] Which decisions can we **defer**? (DB technology, messaging vendor, microservices vs. monolith)
- [ ] Which boundaries are **full**, which are **partial** (to save cost now)?

**Structure**
- [ ] Does the folder/project structure **scream** the domain?
- [ ] Do all dependencies point **inward**? Enforced by project references + architecture tests?
- [ ] Are implementation classes `internal` where possible?
- [ ] Is `Program.cs` the only place that knows about concrete infrastructure?

**Components**
- [ ] No cyclic dependencies between projects/services (ADP)?
- [ ] Stable components abstract, volatile components concrete (SDP/SAP)?
- [ ] Are services split along **axes of change**, not technical functions?

**Testing**
- [ ] Can we test all use cases **without** DB, web server, or Azure?
- [ ] Is there a testing API (use-case level) so tests aren't coupled to UI?

**Operations**
- [ ] Can we deploy with a single command/pipeline?
- [ ] Can we swap a detail (e.g., Blob → S3, SQL → Cosmos) by changing one project?

---

## 6. Interview Questions & Answers

**Q1. What is Clean Architecture?**
An architecture where business rules (entities and use cases) are at the center, independent of frameworks, UI, databases, and external agencies. All source-code dependencies point inward (the Dependency Rule). It makes the system testable, flexible, and maintainable.

**Q2. What is the Dependency Rule?**
Source-code dependencies can only point inward. Inner layers know nothing about outer layers — no class names, functions, or data formats.

**Q3. How does a use case save to the database if it can't reference the DB layer?**
Through Dependency Inversion. The use case layer defines a port (interface like `IOrderRepository`). The infrastructure layer implements it. At runtime, DI injects the implementation. Control flows outward; source dependencies point inward.

**Q4. Entities vs. Use Cases?**
Entities hold enterprise-wide critical business rules that would exist without software (e.g., interest calculation). Use cases hold application-specific rules that orchestrate entities for a particular automated workflow (e.g., "create loan application if credit score ≥ 500").

**Q5. Explain SRP as Uncle Bob defines it in this book.**
"A module should be responsible to one, and only one, actor." It's about separating code that different stakeholders would ask to change, to avoid accidental coupling.

**Q6. What is "Screaming Architecture"?**
The top-level structure should reveal the system's purpose (Orders, Billing, Patients) rather than the framework (Controllers, Models, Views).

**Q7. Why is the database a "detail"?**
Because business rules don't depend on how data is stored. The data model matters; the storage technology is a replaceable mechanism behind an interface.

**Q8. Clean Architecture vs Onion vs Hexagonal?**
They share the same core idea: business logic at the center, infrastructure on the outside, dependencies inward. Hexagonal (Ports & Adapters) emphasizes ports/adapters; Onion emphasizes concentric layers; Clean Architecture unifies them and adds explicit Entities vs. Use Cases layers and the Dependency Rule.

**Q9. Are microservices good architecture?**
Not automatically. Services are a deployment mechanism. If services are split by technical function rather than by axes of change, a single feature still requires changing all of them. Good boundaries matter more than process boundaries. Start with a well-modularized monolith.

**Q10. What are REP, CCP, CRP?**
Component cohesion principles: Reuse/Release Equivalence (release together what's reused together), Common Closure (group things that change together), Common Reuse (don't force users to depend on what they don't use).

**Q11. What are ADP, SDP, SAP?**
Component coupling principles: Acyclic Dependencies (no cycles), Stable Dependencies (depend toward stability), Stable Abstractions (stable components should be abstract).

**Q12. What is the Humble Object pattern?**
Splitting hard-to-test code (UI, DB, network) into a "humble" part with almost no logic and a testable part containing all logic (e.g., Presenter/View, use case/gateway).

**Q13. How do you enforce Clean Architecture in a .NET solution?**
Separate projects with one-directional references, `internal` implementations, a single composition root, and architecture tests (NetArchTest/ArchUnitNET) in CI.

**Q14. When would you NOT use full Clean Architecture?**
For small CRUD apps, prototypes, or short-lived tools — the overhead isn't worth it. Use partial boundaries or vertical slices, and evolve toward stricter boundaries as complexity grows.

---

## 7. Modern Nuance – Don't Over-Engineer

Being senior means knowing *how much* architecture a system needs:

- **CRUD-heavy apps** (admin panels, simple forms): a vertical-slice approach with minimal layering may be enough. Don't create 4 projects + 40 interfaces for a 10-table CRUD app.
- **Mapping overhead is real.** Entity → DTO → ViewModel mapping everywhere has a cost. Use it at real boundaries; skip it where there isn't one.
- **Repository over EF Core debate:** EF Core's `DbContext` is already a Unit of Work + Repository. Many teams use it directly in the Application layer through an `IApplicationDbContext` interface — a pragmatic partial boundary. Both are defensible; be consistent and know the trade-off (you're coupling to EF's LINQ semantics).
- **Vertical Slice Architecture** (organizing by feature, each slice owning its request→response pipeline) pairs very well with Clean Architecture's "screaming" and use-case-centric ideas.
- **Modular monolith first**, microservices when team size, scale, or deployment independence demands it.
- **The principle that never changes:** keep business rules independent and testable; keep details at the edges.

---

## 8. Key Takeaways

1. **Architecture's goal:** minimize the human effort to build and maintain the system.
2. **Structure > behavior** in long-term value. Fight for architecture.
3. **The Dependency Rule:** source dependencies point inward to business rules.
4. **Entities** = enterprise rules; **Use Cases** = application rules. Both framework-free.
5. **DB, Web, and Frameworks are details.** Plug them in at the edges.
6. **Defer decisions** about details as long as possible; keep options open.
7. **Screaming architecture:** structure reveals the domain, not the framework.
8. **SOLID** at class level, **REP/CCP/CRP + ADP/SDP/SAP** at component level.
9. **Humble Objects** make hard-to-test boundaries thin.
10. **Services are not architecture**; boundaries are.
11. **Use the compiler (and architecture tests) to enforce boundaries.**
12. **`Program.cs` is Main** — the dirtiest component, the only one that knows everything.

---

*Previous: [02 – The Clean Coder](02-The-Clean-Coder.md) · Next: [04 – Clean Agile](04-Clean-Agile.md)*
