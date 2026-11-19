# Domain-Driven Design: Tackling Complexity in the Heart of Software

> **Author:** Eric Evans · **Published:** 2003 (the "Blue Book")
> **One-line summary:** The hardest part of most software is the **business domain**, not the technology. So build a **model** of the domain together with domain experts, express it in a **shared language**, put it directly into the **code**, and protect it with clear **boundaries**.
> **Examples in this guide:** C# 12 / .NET 8+, EF Core, ASP.NET Core, MediatR-style handlers, Azure Service Bus.

---

## Table of Contents

1. [What the Book Actually Covers](#1-what-the-book-actually-covers)
2. [Part I – Putting the Domain Model to Work](#2-part-i--putting-the-domain-model-to-work)
   - 2.1 [Knowledge Crunching](#21-knowledge-crunching)
   - 2.2 [Ubiquitous Language](#22-ubiquitous-language)
   - 2.3 [Model-Driven Design: Binding Model and Code](#23-model-driven-design-binding-model-and-code)
3. [Part II – The Building Blocks](#3-part-ii--the-building-blocks)
   - 3.1 [Layered Architecture & Isolating the Domain](#31-layered-architecture--isolating-the-domain)
   - 3.2 [Entities](#32-entities)
   - 3.3 [Value Objects](#33-value-objects)
   - 3.4 [Domain Services](#34-domain-services)
   - 3.5 [Modules](#35-modules)
   - 3.6 [Aggregates](#36-aggregates)
   - 3.7 [Factories](#37-factories)
   - 3.8 [Repositories](#38-repositories)
   - 3.9 [Domain Events (added later)](#39-domain-events-added-later)
4. [Part III – Refactoring Toward Deeper Insight](#4-part-iii--refactoring-toward-deeper-insight)
   - 4.1 [Breakthroughs & Making Implicit Concepts Explicit](#41-breakthroughs--making-implicit-concepts-explicit)
   - 4.2 [Specification Pattern](#42-specification-pattern)
   - 4.3 [Supple Design](#43-supple-design)
   - 4.4 [Analysis Patterns & Design Patterns in the Domain](#44-analysis-patterns--design-patterns-in-the-domain)
5. [Part IV – Strategic Design](#5-part-iv--strategic-design)
   - 5.1 [Bounded Context](#51-bounded-context)
   - 5.2 [Context Map & Integration Patterns](#52-context-map--integration-patterns)
   - 5.3 [Distillation: Core Domain & Subdomains](#53-distillation-core-domain--subdomains)
   - 5.4 [Large-Scale Structure](#54-large-scale-structure)
6. [Full Worked Example – An Order Aggregate in .NET](#6-full-worked-example--an-order-aggregate-in-net)
7. [DDD with Clean Architecture, Microservices, React & Azure](#7-ddd-with-clean-architecture-microservices-react--azure)
8. [Planning Checklist – Before You Build](#8-planning-checklist--before-you-build)
9. [Interview Questions & Answers](#9-interview-questions--answers)
10. [When NOT to Use DDD](#10-when-not-to-use-ddd)
11. [Key Takeaways](#11-key-takeaways)

---

## 1. What the Book Actually Covers

Eric Evans wrote this book after years of building complex business systems (shipping, finance, manufacturing). He noticed that successful projects had one thing in common: **a rich model of the domain, shared by developers and business experts, living directly in the code.**

| Part | Topic | Big idea |
|---|---|---|
| **I. Putting the Domain Model to Work** | Knowledge crunching, Ubiquitous Language, Model-Driven Design | The model is a shared tool for thinking and talking, and the code *is* the model. |
| **II. The Building Blocks of a Model-Driven Design** | Layered architecture, Entities, Value Objects, Services, Modules, Aggregates, Factories, Repositories | The **tactical** patterns: how to express a model in object-oriented code. |
| **III. Refactoring Toward Deeper Insight** | Breakthroughs, implicit concepts, Specification, Supple Design, analysis patterns | Models improve continuously as understanding deepens. |
| **IV. Strategic Design** | Bounded Contexts, Context Maps, Distillation (Core Domain), Large-Scale Structure | The **strategic** patterns: how to organize big systems and many teams. |

**Tactical vs strategic DDD:**
- **Tactical** (Part II) = patterns inside one model (Entity, Aggregate, Repository…). Most developers learn these first.
- **Strategic** (Part IV) = how models and teams relate (Bounded Context, Context Map, Core Domain). Evans later said he wished he had put this part **first**, because it matters more.

> **Central message:** *The heart of software is its ability to solve domain-related problems for its users.* Put your best people and most effort into the **core domain**, and build it as a **model** developed together with domain experts.

---

## 2. Part I – Putting the Domain Model to Work

### 2.1 Knowledge Crunching

**What it is:** Developers and **domain experts** work together, constantly, to digest a mass of information into a **model**: a simplified, useful abstraction of the domain that solves the problem at hand.

- A model is **not** the real world, and it's not a diagram. It's a **selective abstraction**, built for a purpose. (A subway map is a great model: it ignores real geography because riders don't need it.)
- Knowledge crunching is **continuous**. Every conversation, every bug, every new requirement can reveal something the model is missing.
- Developers must **learn the business** deeply. "Just tell me what to build" produces shallow software.

**Real-world example – Evans's PCB (printed circuit board) design story:**
Evans worked with electrical engineers on a tool for circuit-board design. He didn't know electronics. Through repeated sessions, sketching and asking questions, they discovered concepts like *net*, *component instance*, and *signal propagation* that became classes. The software became useful only once the model matched how the engineers actually thought.

**How to do it in practice:**
- **Event Storming** workshops (sticky notes on a wall: domain events in orange, commands in blue, aggregates in yellow).
- **Example mapping** (rules + examples + questions per story).
- Pair a developer with a domain expert for a day.
- Write acceptance tests in the experts' language (Gherkin), which forces precise definitions.

---

### 2.2 Ubiquitous Language

> **Use one shared language, based on the domain model, everywhere: in conversations, documents, diagrams, user stories, and the code itself.**

**The problem it solves:** Business people speak "business"; developers speak "tech." A **translation layer** forms between them. Every translation loses meaning, and bugs hide in the gaps.

| ❌ Translated | ✅ Ubiquitous Language |
|---|---|
| Business: "The underwriter **binds** the policy." Code: `policy.Status = 3; db.Update(policy);` | Code: `policy.Bind(underwriter);` |
| Business: "A **shipment** is **rerouted** when a **leg** is cancelled." Code: `UpdateRecord(cargoId, newPath)` | Code: `cargo.Reroute(newItinerary);` |
| Business: "**Customers** and **Clients**" used for the same thing | Pick one term, define it, use it everywhere |

**Rules:**
- If the code uses a term, experts should understand it. If experts use a term that isn't in the code, the model may be missing something.
- **Changing the language means changing the model** (and the code). Rename classes when the language evolves.
- Experts should object to awkward or wrong terms. Developers should object to vague or ambiguous ones.
- The language is valid **within one Bounded Context** (see 5.1). "Account" can mean different things in Banking vs Identity, and that's fine as long as the boundary is explicit.

**C# example – the code speaks the domain:**

```csharp
// ❌ Technical vocabulary; business rules hidden in a "manager"
public class LoanRecordManager
{
    public void UpdateStatus(int loanId, int statusCode, decimal amt) { /* ... */ }
}

// ✅ Ubiquitous Language: experts could read this method list and nod
public sealed class LoanApplication
{
    public void Submit() { ... }
    public void RequestAdditionalDocuments(DocumentRequest request) { ... }
    public void Approve(Underwriter underwriter, ApprovedTerms terms) { ... }
    public void Decline(Underwriter underwriter, DeclineReason reason) { ... }
    public Loan Disburse(DisbursementAccount account) { ... }
}
```

**Tip:** keep a **glossary** (`docs/ubiquitous-language.md`) per Bounded Context, and review it in refinement sessions.

---

### 2.3 Model-Driven Design: Binding Model and Code

> **Design a portion of the software system to reflect the domain model very literally, so the mapping is obvious.**

- **Don't** have an "analysis model" (diagrams by analysts) and a separate "design model" (code by developers). They drift apart and the analysis model becomes useless.
- **One model** serves both analysis and design. If the model can't be implemented well, change the model.
- **Hands-on modelers:** the people who model must also code, and the people who code must take part in modeling. Otherwise, insights are lost.
- This needs a language/paradigm that can express a model, which is why DDD is usually done with **OO** (or a functional style with rich types; see [Functional Design](../Rober%20C%20Martin/07-Functional-Design.md)).

**Anti-pattern: the Anemic Domain Model** (named later by Martin Fowler): classes with only getters/setters, and all logic in "service" classes. It looks like a model but isn't one. Behavior and data are separated, so rules get duplicated and scattered.

```csharp
// ❌ Anemic: a data bag + logic elsewhere
public class Order { public List<OrderLine> Lines { get; set; } = []; public string Status { get; set; } = "New"; }
public class OrderService
{
    public void Submit(Order o)
    {
        if (o.Lines.Count == 0) throw new Exception("Empty");
        o.Status = "Submitted";            // any other code can also set Status to anything
    }
}

// ✅ Rich model: behavior and invariants live with the data
public sealed class Order
{
    private readonly List<OrderLine> _lines = [];
    public IReadOnlyList<OrderLine> Lines => _lines;
    public OrderStatus Status { get; private set; } = OrderStatus.Draft;

    public void Submit()
    {
        if (_lines.Count == 0) throw new DomainException("An order needs at least one line to be submitted.");
        if (Status != OrderStatus.Draft) throw new DomainException($"Only draft orders can be submitted (was {Status}).");
        Status = OrderStatus.Submitted;
    }
}
```

---

## 3. Part II – The Building Blocks

### 3.1 Layered Architecture & Isolating the Domain

Evans recommends separating the system into layers so the domain logic is **isolated** from UI, application coordination, and infrastructure:

| Layer | Responsibility | .NET project (typical) |
|---|---|---|
| **User Interface / Presentation** | Show information, interpret user commands | ASP.NET Core API + React SPA |
| **Application** | Coordinate tasks; thin; no business rules; transactions, security | `Shop.Application` (use-case handlers) |
| **Domain** | Business concepts, rules, state. **The heart of the software.** | `Shop.Domain` |
| **Infrastructure** | Persistence, messaging, email, external APIs | `Shop.Infrastructure` |

**The key rule:** the domain layer must be **free of technical concerns** so the model can be expressed clearly and tested easily.

Modern practice usually **inverts** Evans's original top-to-bottom layering so that infrastructure depends on the domain (Hexagonal / Onion / Clean Architecture). See [03 – Clean Architecture](../Rober%20C%20Martin/03-Clean-Architecture.md). The spirit is identical: **protect the domain.**

**Anti-pattern: Smart UI.** Business logic inside the UI (code-behind, React components, controllers). Evans admits it's acceptable for **very simple** apps built by teams without modeling skills, but it can't grow into a rich domain. You can't mix the two approaches easily, so choose deliberately.

---

### 3.2 Entities

> An **Entity** is an object defined by its **identity**, which stays the same through time and through changes to its attributes.

**Real-world example:** You. Your name, address, and phone number may all change; you're still the same person. A **Customer**, **Order**, **Bank Account**, or **Cargo** is an entity.

**Rules:**
- Give entities a stable, unique **ID** (often generated early, e.g., a `Guid`/UUIDv7, not relying on the DB).
- **Equality = same ID**, not same attributes.
- Keep entities focused on **identity + lifecycle + behavior**. Move descriptive attributes into value objects.

```csharp
public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
{
    public TId Id { get; protected init; } = default!;

    public bool Equals(Entity<TId>? other) =>
        other is not null && GetType() == other.GetType() && EqualityComparer<TId>.Default.Equals(Id, other.Id);

    public override bool Equals(object? obj) => obj is Entity<TId> e && Equals(e);
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
}

// Strongly-typed ID avoids mixing up a CustomerId with an OrderId
public readonly record struct CustomerId(Guid Value)
{
    public static CustomerId New() => new(Guid.CreateVersion7());   // .NET 9+; use Guid.NewGuid() on .NET 8
}

public sealed class Customer : Entity<CustomerId>
{
    public CustomerName Name { get; private set; }
    public EmailAddress Email { get; private set; }

    public Customer(CustomerId id, CustomerName name, EmailAddress email) =>
        (Id, Name, Email) = (id, name, email);

    public void ChangeEmail(EmailAddress newEmail) => Email = newEmail;   // still the same customer
}
```

---

### 3.3 Value Objects

> A **Value Object** describes a characteristic of something. It has **no identity**; it's defined entirely by its **attributes**. It should be **immutable**.

**Real-world example:** A ₹500 note. You don't care *which* ₹500 note you get, only its value. Other examples: **Money, Address, DateRange, EmailAddress, Temperature, Color, Quantity, GeoLocation**.

**Rules:**
- **Equality = all attributes equal.**
- **Immutable:** to "change" one, replace it with a new one.
- **Self-validating:** an invalid value object can't exist.
- **Side-effect-free behavior:** `money.Add(other)` returns new money.
- **Prefer value objects over primitives** (avoid "primitive obsession"): an `EmailAddress` beats a `string`.

C# `record` types are a near-perfect fit:

```csharp
public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
            throw new DomainException("Currency must be a 3-letter ISO code.");
        Amount = decimal.Round(amount, 2);
        Currency = currency.ToUpperInvariant();
    }

    public static Money Zero(string currency) => new(0, currency);

    public Money Add(Money other)      { EnsureSameCurrency(other); return new(Amount + other.Amount, Currency); }
    public Money Subtract(Money other) { EnsureSameCurrency(other); return new(Amount - other.Amount, Currency); }
    public Money Multiply(decimal factor) => new(Amount * factor, Currency);

    private void EnsureSameCurrency(Money other)
    {
        if (other.Currency != Currency) throw new DomainException($"Cannot combine {Currency} and {other.Currency}.");
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}

public sealed record EmailAddress
{
    public string Value { get; }
    public EmailAddress(string value)
    {
        if (!System.Net.Mail.MailAddress.TryCreate(value, out _))
            throw new DomainException($"'{value}' is not a valid email address.");
        Value = value.Trim().ToLowerInvariant();
    }
}
```

> Note: positional records (`record DateRange(DateOnly Start, DateOnly End)`) can't validate in their primary constructor easily. For self-validating value objects, write an explicit constructor:

```csharp
public sealed record DateRange
{
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public DateRange(DateOnly start, DateOnly end)
    {
        if (end < start) throw new DomainException("End date must be on or after start date.");
        (Start, End) = (start, end);
    }

    public int Days => End.DayNumber - Start.DayNumber + 1;
    public bool Contains(DateOnly d) => d >= Start && d <= End;
    public bool Overlaps(DateRange other) => Start <= other.End && other.Start <= End;
}
```

**Persisting value objects with EF Core:** owned types or complex types (EF Core 8+):

```csharp
builder.OwnsOne(o => o.Total, m =>
{
    m.Property(x => x.Amount).HasColumnName("TotalAmount").HasPrecision(18, 2);
    m.Property(x => x.Currency).HasColumnName("TotalCurrency").HasMaxLength(3);
});

builder.Property(c => c.Email)
       .HasConversion(e => e.Value, v => new EmailAddress(v))
       .HasMaxLength(256);
```

**Entity or Value Object?** Ask: *"Do I care which one it is, or only what it is?"* An `Address` is a value object in an online shop (only the content matters) but could be an entity for a postal service (each delivery point has a tracked identity). **The context decides.**

---

### 3.4 Domain Services

> When an important domain operation **doesn't naturally belong** to any entity or value object, model it as a **Service**: a stateless operation named in the Ubiquitous Language.

**Real-world example:** **Transferring money** between two accounts. Does it belong to the source account or the target account? Neither alone. It's a domain service: `FundsTransferService.Transfer(from, to, amount)`.

**Good domain service characteristics:**
1. The operation relates to a **domain concept** that isn't a natural part of an entity/value object.
2. Its interface is defined in terms of **domain model elements**.
3. It's **stateless**.

**Three kinds of services (don't mix them up):**

| Kind | Contains | Example |
|---|---|---|
| **Domain service** | Business rules | `FundsTransferService`, `PricingService`, `RoutingService` |
| **Application service** | Orchestration, transactions, security; **no business rules** | `TransferFundsHandler` (loads accounts, calls domain, saves, publishes) |
| **Infrastructure service** | Technical capability | `SmtpEmailSender`, `BlobDocumentStore` |

```csharp
// Domain service – pure business rule spanning two aggregates
public sealed class FundsTransferService
{
    public void Transfer(BankAccount from, BankAccount to, Money amount)
    {
        if (from.Id == to.Id) throw new DomainException("Cannot transfer to the same account.");
        from.Withdraw(amount);
        to.Deposit(amount);
    }
}

// Application service – coordinates; contains no business rules itself
public sealed class TransferFundsHandler(IBankAccountRepository accounts, FundsTransferService transfer, IUnitOfWork uow)
{
    public async Task Handle(TransferFunds cmd, CancellationToken ct)
    {
        var from = await accounts.GetAsync(cmd.FromAccountId, ct);
        var to   = await accounts.GetAsync(cmd.ToAccountId, ct);
        transfer.Transfer(from, to, new Money(cmd.Amount, cmd.Currency));
        await uow.SaveChangesAsync(ct);
    }
}
```

> **Warning:** overusing services leads back to the Anemic Domain Model. First try to put behavior on entities and value objects.

---

### 3.5 Modules

> **Modules** (namespaces/folders/projects) should tell the story of the domain. Group by **domain concepts** with high cohesion and low coupling, not by technical type.

```
❌ Technical grouping              ✅ Domain grouping (Modules)
Shop.Domain/                       Shop.Domain/
  Entities/                          Ordering/      (Order, OrderLine, OrderStatus, OrderPlaced)
  ValueObjects/                      Catalog/       (Product, Sku, PriceList)
  Services/                          Customers/     (Customer, CustomerName, LoyaltyTier)
  Enums/                             Shipping/      (Shipment, TrackingNumber, Carrier)
                                     SharedKernel/  (Money, EmailAddress, Entity<TId>)
```

Module names are part of the Ubiquitous Language. Refactor modules when the model changes.

---

### 3.6 Aggregates

This is the most important (and most misunderstood) tactical pattern.

> An **Aggregate** is a cluster of entities and value objects treated as **one unit for data changes**. It has one **root** entity. Outside objects may reference only the root. The aggregate enforces its **invariants** (business rules that must always be true) inside its **consistency boundary**.

**Real-world example:** A **car**. You interact with the car (the root): drive it, service it. You don't reach in and swap a single wheel's tire pressure independently; the car's rules (e.g., all tires in spec) are checked as a whole. An external system tracks "car #123", not "wheel #456 of some car."

**Rules (Evans + later refinements by Vaughn Vernon):**
1. **Protect invariants inside the boundary.** Every change goes through the root, which checks the rules.
2. **External objects hold references only to the root** (by ID). They may not hold references to internal entities.
3. **Only roots are loaded from repositories.** One repository per aggregate root.
4. **Delete the root → everything inside goes.**
5. **One transaction modifies one aggregate** (Vernon's guidance). Changes to *other* aggregates happen through **domain events** with **eventual consistency**.
6. **Design small aggregates.** Big aggregates cause locking conflicts and slow loading.
7. **Reference other aggregates by ID**, not object reference.

```csharp
public sealed class Order : AggregateRoot<OrderId>
{
    private const int MaxLines = 50;
    private readonly List<OrderLine> _lines = [];

    public CustomerId CustomerId { get; }             // reference to another aggregate BY ID
    public OrderStatus Status { get; private set; }
    public DateTimeOffset? PlacedAt { get; private set; }
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();
    public Money Total => _lines.Select(l => l.Subtotal).Aggregate(Money.Zero(Currency), (a, b) => a.Add(b));
    public string Currency { get; }

    private Order(OrderId id, CustomerId customerId, string currency)
    {
        Id = id; CustomerId = customerId; Currency = currency; Status = OrderStatus.Draft;
    }

    public static Order StartFor(CustomerId customerId, string currency) => new(OrderId.New(), customerId, currency);

    // Invariant: max 50 lines, one line per product, only while Draft, positive quantity, same currency
    public void AddItem(ProductId productId, int quantity, Money unitPrice)
    {
        EnsureDraft();
        if (quantity <= 0) throw new DomainException("Quantity must be positive.");
        if (unitPrice.Currency != Currency) throw new DomainException("Price currency must match order currency.");

        var existing = _lines.FirstOrDefault(l => l.ProductId == productId);
        if (existing is not null) { existing.IncreaseQuantity(quantity); return; }

        if (_lines.Count >= MaxLines) throw new DomainException($"An order can have at most {MaxLines} lines.");
        _lines.Add(new OrderLine(OrderLineId.New(), productId, quantity, unitPrice));
    }

    public void RemoveItem(ProductId productId)
    {
        EnsureDraft();
        _lines.RemoveAll(l => l.ProductId == productId);
    }

    public void Place(DateTimeOffset now)
    {
        EnsureDraft();
        if (_lines.Count == 0) throw new DomainException("Cannot place an empty order.");
        Status = OrderStatus.Placed;
        PlacedAt = now;
        Raise(new OrderPlaced(Id, CustomerId, Total, now));   // other aggregates react via events
    }

    private void EnsureDraft()
    {
        if (Status != OrderStatus.Draft) throw new DomainException($"Order is {Status}; it can no longer be changed.");
    }
}

// Internal entity: only reachable through the Order root. Note the 'internal' members.
public sealed class OrderLine : Entity<OrderLineId>
{
    public ProductId ProductId { get; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; }
    public Money Subtotal => UnitPrice.Multiply(Quantity);

    internal OrderLine(OrderLineId id, ProductId productId, int quantity, Money unitPrice) =>
        (Id, ProductId, Quantity, UnitPrice) = (id, productId, quantity, unitPrice);

    internal void IncreaseQuantity(int by) => Quantity += by;
}
```

**Choosing aggregate boundaries – ask:**
- What **must be consistent immediately** (in the same transaction)? → same aggregate.
- What can be consistent **a few seconds later**? → separate aggregates + domain events.
- Example: "Order total must equal sum of lines" → same aggregate. "Customer loyalty points increase after an order is placed" → different aggregate, updated eventually via `OrderPlaced`.

**Concurrency:** use optimistic concurrency on the root (EF Core `RowVersion` / Cosmos DB `_etag`), so two users editing the same order don't silently overwrite each other.

```csharp
builder.Property<byte[]>("RowVersion").IsRowVersion();   // SQL Server optimistic concurrency
```

---

### 3.7 Factories

> When creating an aggregate is **complex** or reveals too much internal structure, encapsulate creation in a **Factory**.

- The factory produces a **valid** object (all invariants satisfied) in one atomic step.
- Options: a **static factory method** on the root (`Order.StartFor(...)`), a **factory method on another aggregate** (`Customer.PlaceOrder(...)`), or a separate **factory class** when creation needs other services.
- **Reconstitution** (loading from the DB) is different from creation: it must not re-run "creation" rules or raise "created" events. EF Core uses private constructors for this.

```csharp
// Factory method on another aggregate expresses a domain rule: only active customers can start orders
public sealed class Customer : AggregateRoot<CustomerId>
{
    public bool IsActive { get; private set; }
    // ... other members ...

    public Order StartOrder(string currency)
    {
        if (!IsActive) throw new DomainException("Inactive customers cannot place orders.");
        return Order.StartFor(Id, currency);
    }
}

// Separate factory when creation needs outside knowledge
public sealed class InsuranceQuoteFactory(IRiskRatingService risk, IClock clock)
{
    public async Task<Quote> CreateAsync(Applicant applicant, Vehicle vehicle, CancellationToken ct)
    {
        var rating = await risk.RateAsync(applicant, vehicle, ct);
        return Quote.Create(applicant.Id, vehicle, rating, validUntil: clock.Today.AddDays(30));
    }
}
```

---

### 3.8 Repositories

> A **Repository** gives the illusion of an **in-memory collection of all aggregates of a type**. It hides how they're stored and loaded.

**Rules:**
- **One repository per aggregate root.** No `OrderLineRepository`.
- The interface lives in the **domain** (or application) layer, using domain language. The implementation lives in **infrastructure**.
- Return **whole aggregates**, fully loaded (consistent).
- Keep query methods meaningful in the domain (`GetOverdueFor(customerId)`), not generic `IQueryable` leaks.
- **Reads for screens/reports** don't need to go through aggregates; use a separate read model/query (CQRS).

```csharp
// Domain layer – collection-like, domain language
public interface IOrderRepository
{
    Task<Order?> FindAsync(OrderId id, CancellationToken ct);
    Task<IReadOnlyList<Order>> PlacedButUnpaidSince(DateTimeOffset cutoff, CancellationToken ct);
    void Add(Order order);
    void Remove(Order order);
}

// Infrastructure layer – EF Core implementation
internal sealed class EfOrderRepository(ShopDbContext db) : IOrderRepository
{
    public Task<Order?> FindAsync(OrderId id, CancellationToken ct) =>
        db.Orders.Include(o => o.Lines).SingleOrDefaultAsync(o => o.Id == id, ct);

    public async Task<IReadOnlyList<Order>> PlacedButUnpaidSince(DateTimeOffset cutoff, CancellationToken ct) =>
        await db.Orders.Include(o => o.Lines)
                       .Where(o => o.Status == OrderStatus.Placed && o.PlacedAt < cutoff)
                       .ToListAsync(ct);

    public void Add(Order order) => db.Orders.Add(order);
    public void Remove(Order order) => db.Orders.Remove(order);
}

// Read side (CQRS) – query straight into a DTO for the React screen, no aggregate needed
public sealed class OrderSummaryQuery(ShopDbContext db)
{
    public Task<List<OrderSummaryDto>> ForCustomer(Guid customerId, CancellationToken ct) =>
        db.Orders.AsNoTracking()
          .Where(o => o.CustomerId == new CustomerId(customerId))
          .OrderByDescending(o => o.PlacedAt)
          .Select(o => new OrderSummaryDto(o.Id.Value, o.Status.ToString(), o.Lines.Count))
          .ToListAsync(ct);
}
```

---

### 3.9 Domain Events (added later)

Domain Events were **not** one of the original Blue Book building blocks. Evans added them in later writing (the *DDD Reference*, 2015) because the community found them essential.

> A **Domain Event** records something **that happened** in the domain that domain experts care about. Named in the past tense: `OrderPlaced`, `PaymentReceived`, `CargoMisdirected`.

**Uses:**
- Trigger side effects in **other aggregates** (eventual consistency).
- Integrate **other Bounded Contexts** (via a message bus).
- Build **audit logs** and **event-sourced** models.

```csharp
public interface IDomainEvent { DateTimeOffset OccurredAt { get; } }
public sealed record OrderPlaced(OrderId OrderId, CustomerId CustomerId, Money Total, DateTimeOffset OccurredAt) : IDomainEvent;

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvents where TId : notnull
{
    private readonly List<IDomainEvent> _events = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _events;
    protected void Raise(IDomainEvent e) => _events.Add(e);
    public void ClearDomainEvents() => _events.Clear();
}

// Dispatch after a successful save (EF Core SaveChanges interceptor or Unit of Work)
public sealed class ShopDbContext(DbContextOptions<ShopDbContext> options, IDomainEventDispatcher dispatcher)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders => Set<Order>();

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        var roots = ChangeTracker.Entries<IHasDomainEvents>().Select(e => e.Entity).ToList();
        var events = roots.SelectMany(r => r.DomainEvents).ToList();

        await base.SaveChangesAsync(ct);
        roots.ForEach(r => r.ClearDomainEvents());
        await dispatcher.DispatchAsync(events, ct);    // in-process handlers, or write to an outbox
    }
}

// Another aggregate reacts (eventual consistency)
public sealed class AwardLoyaltyPointsOnOrderPlaced(ICustomerRepository customers, IUnitOfWork uow)
    : IDomainEventHandler<OrderPlaced>
{
    public async Task Handle(OrderPlaced e, CancellationToken ct)
    {
        var customer = await customers.GetAsync(e.CustomerId, ct);
        customer.AwardPointsFor(e.Total);
        await uow.SaveChangesAsync(ct);
    }
}
```

> **Reliability tip:** to publish events to **Azure Service Bus** without losing them, use the **Transactional Outbox** pattern: save events to an `Outbox` table in the same DB transaction, and have a background worker publish them.

---

## 4. Part III – Refactoring Toward Deeper Insight

### 4.1 Breakthroughs & Making Implicit Concepts Explicit

- Models improve through **continuous refactoring**, not just code-level refactoring (rename, extract) but **model-level** refactoring as understanding deepens.
- Sometimes a series of small refactorings leads to a **breakthrough**: a new model that suddenly makes the design much simpler and more powerful.
- **Evans's syndicated loan story:** a team modeling bank loan syndicates struggled with complex share calculations. They eventually realized that **"share pie"** (how a loan or fee is divided among lenders) was a central concept, separate from the loan itself. Making it explicit simplified everything.

**Listen for implicit concepts:**
- Experts use a word that isn't in the code.
- Awkward code with many `if`s around some unnamed idea.
- Contradictions between experts (often two different concepts or contexts).
- Reading books in the domain (accounting, logistics, insurance).

**Making constraints, processes, and rules explicit:**

```csharp
// ❌ Implicit: an overbooking rule hidden inside a method
public void Book(Voyage voyage, Cargo cargo)
{
    if (voyage.BookedCargoSize + cargo.Size > voyage.Capacity * 1.1) throw new DomainException("Full");
    voyage.Add(cargo);
}

// ✅ Explicit: the policy is a named domain concept, testable on its own, swappable
public interface IOverbookingPolicy { bool IsAllowed(Cargo cargo, Voyage voyage); }

public sealed class TenPercentOverbookingPolicy : IOverbookingPolicy
{
    public bool IsAllowed(Cargo cargo, Voyage voyage) =>
        voyage.BookedCargoSize + cargo.Size <= voyage.Capacity * 1.1m;
}

public sealed class BookingService(IOverbookingPolicy overbooking)
{
    public void Book(Voyage voyage, Cargo cargo)
    {
        if (!overbooking.IsAllowed(cargo, voyage))
            throw new DomainException("Booking would exceed the overbooking allowance.");
        voyage.Add(cargo);
    }
}
```

(This "overbooking policy" is close to Evans's own example in the book.)

---

### 4.2 Specification Pattern

> A **Specification** is a predicate object that states whether an object satisfies some criteria. It makes **business rules explicit, named, combinable, and reusable**.

Three uses (from the book):
1. **Validation:** does this object meet the rule?
2. **Selection/querying:** find all objects that meet the rule.
3. **Building to order:** create something that meets the rule.

```csharp
public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();   // usable by EF Core (translated to SQL)

    public bool IsSatisfiedBy(T candidate) => ToExpression().Compile()(candidate);

    public Specification<T> And(Specification<T> other) => new AndSpecification<T>(this, other);
}

internal sealed class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var l = left.ToExpression();
        var r = right.ToExpression();
        var param = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(l, param),
            Expression.Invoke(r, param));
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}

// Named business rules in the Ubiquitous Language
public sealed class OverdueInvoiceSpecification(DateOnly today) : Specification<Invoice>
{
    public override Expression<Func<Invoice, bool>> ToExpression() => i => !i.IsPaid && i.DueDate < today;
}

public sealed class HighValueInvoiceSpecification(decimal threshold) : Specification<Invoice>
{
    public override Expression<Func<Invoice, bool>> ToExpression() => i => i.Amount >= threshold;
}

// 1. Validation
var needsEscalation = new OverdueInvoiceSpecification(today).And(new HighValueInvoiceSpecification(10_000));
if (needsEscalation.IsSatisfiedBy(invoice)) { /* escalate to collections */ }

// 2. Selection (translated to SQL by EF Core)
var toEscalate = await db.Invoices.Where(needsEscalation.ToExpression()).ToListAsync(ct);
```

> For production use, the **Ardalis.Specification** NuGet package provides a well-tested implementation.

---

### 4.3 Supple Design

A **supple design** is pleasant to work with: it invites change and makes the effects of code obvious. Evans's patterns for it:

| Pattern | Meaning | C# example |
|---|---|---|
| **Intention-Revealing Interfaces** | Name classes and methods for *what* they do and *why*, not *how* | `order.Place()` not `order.SetStatusAndRaiseEvent()` |
| **Side-Effect-Free Functions** | Put as much logic as possible in functions that return results without changing state (especially on value objects) | `Money.Add()` returns new `Money` |
| **Assertions** | State the post-conditions and invariants explicitly (in code, tests, or contracts) | Guard clauses + unit tests that document rules |
| **Conceptual Contours** | Split the design along the domain's natural seams, so that changes stay local | Separate `Pricing` from `Inventory` because they change for different reasons |
| **Standalone Classes** | Minimize dependencies; a class understandable on its own is easier to use | Value objects with zero dependencies |
| **Closure of Operations** | An operation whose return type equals its argument types (`T op T → T`) composes nicely | `Money + Money → Money`, `DateRange.Intersect(DateRange) → DateRange` |
| **Declarative Design** | Express rules declaratively (e.g., combinable specifications) | `spec1.And(spec2)` |

```csharp
// Closure of operations + side-effect-free functions: Paint mixing (Evans's example)
public sealed record Paint(decimal Volume, Pigment Pigment)
{
    public Paint MixedWith(Paint other) =>
        new(Volume + other.Volume, Pigment.MixedWith(other.Pigment, Volume / (Volume + other.Volume)));
}

public sealed record Pigment(int Red, int Yellow, int Blue)
{
    public Pigment MixedWith(Pigment other, decimal ratio) => new(
        Blend(Red, other.Red, ratio), Blend(Yellow, other.Yellow, ratio), Blend(Blue, other.Blue, ratio));

    private static int Blend(int a, int b, decimal ratio) => (int)Math.Round(a * ratio + b * (1 - ratio));
}
```

---

### 4.4 Analysis Patterns & Design Patterns in the Domain

- **Analysis patterns** (Martin Fowler's book *Analysis Patterns*) are reusable *domain* models, e.g., **Account/Transaction** (balance derived from entries), **Party**, **Accountability**, **Quantity/Unit**. Use them as a starting point, then adapt to your domain.
- **Design patterns as domain patterns:**
  - **Strategy → Policy:** a business rule that varies (`IOverbookingPolicy`, `IRoutingPolicy`, `ITaxPolicy`).
  - **Composite:** domain structures that are trees (route made of legs and sub-routes; product bundles; org charts).

```csharp
// Account/Entry analysis pattern: balance is derived, entries are immutable history
public sealed class LedgerAccount : Entity<Guid>
{
    private readonly List<LedgerEntry> _entries = [];
    public IReadOnlyList<LedgerEntry> Entries => _entries;
    public Money Balance(string currency) =>
        _entries.Aggregate(Money.Zero(currency), (sum, e) => sum.Add(e.Amount));

    public void Post(LedgerEntry entry) => _entries.Add(entry);   // never edit, only append
}
public sealed record LedgerEntry(Money Amount, DateTimeOffset PostedAt, string Description);
```

---

## 5. Part IV – Strategic Design

### 5.1 Bounded Context

> A **Bounded Context** is an explicit boundary within which a particular **model** (and its Ubiquitous Language) applies consistently.

**Why:** In a large system, a **single unified model is impossible** (and undesirable). The word "Product" means different things to different departments:

| Context | "Product" means… | Cares about |
|---|---|---|
| **Catalog** | Something we display | Name, description, images, categories, SEO |
| **Pricing** | Something we price | Base price, discounts, price lists, currency |
| **Inventory** | Something we stock | SKU, warehouse location, quantity on hand, reorder level |
| **Shipping** | Something we ship | Weight, dimensions, hazardous flag |
| **Accounting** | Something we sell | Revenue account, tax category |

Forcing all of this into one `Product` class creates a huge, fragile "god class" that every team fights over. Instead, **each context has its own model**, linked by a shared ID (`ProductId` / SKU).

**In code:**
- Each Bounded Context = its own **module, project, or service**, with its own model, DB schema (ideally), and team.
- Translation happens **at the boundaries**.

```
src/
  Catalog/        Catalog.Domain, Catalog.Application, Catalog.Infrastructure, Catalog.Api
  Ordering/       Ordering.Domain, Ordering.Application, ...
  Inventory/      ...
  Shipping/       ...
  SharedKernel/   (tiny: Money, ProductId, base types)
```

**Bounded Context ≠ microservice**, though they often align. A modular monolith can contain several Bounded Contexts as modules. One context should usually **not** be split across several services that share a model.

**Continuous Integration (within a context):** everyone working in a context must merge and test often so the model doesn't fragment silently.

---

### 5.2 Context Map & Integration Patterns

> A **Context Map** shows all Bounded Contexts, their relationships, and how they translate between models. It documents the **real** situation, politics included.

**Relationship patterns:**

| Pattern | Meaning | When |
|---|---|---|
| **Shared Kernel** | Two teams share a small, explicitly agreed subset of the model (and its code). Changes require both teams' agreement. | Closely cooperating teams; keep the kernel tiny (e.g., `Money`, IDs). |
| **Customer/Supplier** | Upstream (supplier) team serves a downstream (customer) team; downstream needs are part of upstream planning; joint acceptance tests. | Teams in the same organization with a clear dependency. |
| **Conformist** | Downstream simply adopts the upstream model as-is, because upstream won't accommodate them. | Upstream is a big vendor/legacy you can't influence, and its model is acceptable. |
| **Anticorruption Layer (ACL)** | Downstream builds a **translation layer** so the foreign model doesn't pollute its own. | Integrating with legacy systems or external APIs with a poor or different model. **Very common and important.** |
| **Separate Ways** | No integration at all; contexts solve their needs independently. | Integration cost > benefit. |
| **Open Host Service (OHS)** | Upstream defines a protocol/API open to all consumers, rather than custom integrations per consumer. | Many downstream consumers. |
| **Published Language** | A well-documented shared language for exchange (often used with OHS). | Industry standards (e.g., financial message formats), or your own versioned event schemas/OpenAPI contracts. |
| **Big Ball of Mud** *(added later)* | Mark a messy area on the map honestly, and put a boundary around it. | Legacy systems; don't let the mud spread. |

**Example Context Map for an online shop:**

```
          [Identity] ──(OHS/PL: OpenID Connect)──▶ all contexts
               
 [Catalog] ──(OHS, Published Language: ProductPublished event)──▶ [Ordering]
                                                                     │
 [Pricing] ◀──(Customer/Supplier)── [Ordering] ──(OrderPlaced event)──▶ [Inventory]
                                         │                               │
                                         ▼                               ▼
                         [Payments] ──ACL──▶ «Stripe API»          [Shipping] ──ACL──▶ «Legacy WMS (Big Ball of Mud)»
                                         
 [Ordering] ◀──Shared Kernel (Money, ProductId)──▶ [Pricing]
```

#### Anticorruption Layer in C#

```csharp
// The legacy warehouse system has its own (messy) model:
public sealed class WmsStockRecordDto      // generated from a SOAP/REST client – NOT our model
{
    public string ITEM_CD { get; set; } = "";
    public string WHSE { get; set; } = "";
    public string QTY_OH { get; set; } = "";   // quantity on hand, as a string 😬
    public string STAT { get; set; } = "";     // "A", "I", "D"
}

// Our Inventory model speaks our language:
public sealed record StockLevel(Sku Sku, WarehouseCode Warehouse, int QuantityOnHand, bool IsSellable);

public interface IWarehouseStock { Task<StockLevel> GetAsync(Sku sku, WarehouseCode warehouse, CancellationToken ct); }

// ACL: facade + adapter + translator. The only place that knows the legacy model exists.
internal sealed class LegacyWmsStockAdapter(LegacyWmsClient client) : IWarehouseStock
{
    public async Task<StockLevel> GetAsync(Sku sku, WarehouseCode warehouse, CancellationToken ct)
    {
        var dto = await client.GetStockAsync(sku.Value, warehouse.Value, ct);
        return WmsTranslator.ToStockLevel(dto);
    }
}

internal static class WmsTranslator
{
    public static StockLevel ToStockLevel(WmsStockRecordDto dto) => new(
        new Sku(dto.ITEM_CD.Trim()),
        new WarehouseCode(dto.WHSE.Trim()),
        int.TryParse(dto.QTY_OH, out var q) ? Math.Max(q, 0) : 0,
        IsSellable: dto.STAT == "A");
}
```

---

### 5.3 Distillation: Core Domain & Subdomains

> **Distillation** separates the essential from the rest, so the team can focus its best effort on what makes the business **unique**.

| Kind | Meaning | Strategy | Example (online shop) |
|---|---|---|---|
| **Core Domain** | What gives the business its **competitive advantage**. Complex and valuable. | Put your **best developers** here; rich model; full DDD; build in-house | Personalized pricing & recommendations; same-day delivery promise engine |
| **Supporting Subdomain** | Necessary and specific to the business, but not a differentiator | Simpler design; can be built by less senior team or outsourced | Returns handling, internal admin tools |
| **Generic Subdomain** | Solved problem, same for everyone | **Buy or use off-the-shelf / SaaS**; don't build | Identity (Entra ID / Azure AD B2C), payments (Stripe), email (Azure Communication Services), accounting |

**Supporting tools from the book:**
- **Domain Vision Statement:** a one-page description of the core domain and its value. Guides decisions.
- **Highlighted Core:** mark the core elements in docs/code so everyone knows what matters most.
- **Cohesive Mechanisms:** extract complex but generic algorithms (graph traversal, rule engines) into separate frameworks, so the core model stays expressive.
- **Segregated Core:** refactor so the core concepts are physically separated from supporting code.
- **Abstract Core:** model the most fundamental concepts as abstractions in their own module.

**For a PM / architect:** this is a **budgeting tool**. Don't spend your best people's time building a custom login system (generic) while the pricing engine (core) is built by whoever is free.

**Example Domain Vision Statement:**

> *"Our core domain is the **Delivery Promise**: given a basket and an address, we commit to a delivery window that we keep 99% of the time. The model predicts stock availability, picking time, and carrier capacity, and replans when reality changes. This is where we beat competitors. Everything else (catalog, payments, accounts) exists to support it."*

---

### 5.4 Large-Scale Structure

For very large systems, Evans describes ways to give the whole system a shape that people can understand:

- **Evolving Order:** let the large-scale structure evolve with the system; don't impose a rigid framework up front.
- **System Metaphor:** a shared metaphor (from XP) that helps everyone understand the overall system.
- **Responsibility Layers:** organize the domain into conceptual layers by responsibility, e.g. *Potential* (what resources exist), *Operations* (what's being done now), *Decision Support* (analysis/planning), *Policy* (rules and goals). Higher layers depend on lower ones.
- **Knowledge Level:** separate objects that describe *rules/types/configuration* (the knowledge level) from objects that describe *operational instances*. For example, `EmployeeType` configuration that defines what a `Employee` may do, so behavior can be configured instead of hard-coded.
- **Pluggable Component Framework:** a mature, abstract core model with plug-in components (rarely achieved; needs a deeply understood domain).

**Bringing strategy together:** context mapping, distillation, and large-scale structure complement each other. Strategic decisions should involve the **teams** that will live with them, not be imposed by an ivory-tower architect. Evans recommends a "**minimalism**" principle: use only as much structure as actually helps.

---

## 6. Full Worked Example – An Order Aggregate in .NET

**Bounded Context:** *Ordering*. **Use case:** "Place an order."

```
Ordering/
  Ordering.Domain/
    Orders/       Order.cs, OrderLine.cs, OrderId.cs, OrderStatus.cs, Events/OrderPlaced.cs, IOrderRepository.cs
    Customers/    CustomerId.cs (reference only; Customer lives in the Customers context)
    Pricing/      IPriceQuote.cs (port to the Pricing context)
  Ordering.Application/
    PlaceOrder/   PlaceOrderCommand.cs, PlaceOrderHandler.cs
  Ordering.Infrastructure/
    Persistence/  OrderingDbContext.cs, OrderConfiguration.cs, EfOrderRepository.cs, Outbox/
    Pricing/      PricingApiAdapter.cs (ACL to the Pricing context)
    Messaging/    ServiceBusOutboxPublisher.cs
  Ordering.Api/
    Endpoints/OrderEndpoints.cs, Program.cs
```

### Application service

```csharp
public sealed record PlaceOrderCommand(Guid CustomerId, string Currency, IReadOnlyList<PlaceOrderItem> Items);
public sealed record PlaceOrderItem(Guid ProductId, int Quantity);

public sealed class PlaceOrderHandler(
    IOrderRepository orders,
    IPriceQuote prices,          // port to another Bounded Context (implemented by an ACL adapter)
    IUnitOfWork unitOfWork,
    TimeProvider time)
{
    public async Task<Guid> Handle(PlaceOrderCommand cmd, CancellationToken ct)
    {
        var order = Order.StartFor(new CustomerId(cmd.CustomerId), cmd.Currency);

        foreach (var item in cmd.Items)
        {
            var productId = new ProductId(item.ProductId);
            var unitPrice = await prices.CurrentPriceAsync(productId, cmd.Currency, ct);
            order.AddItem(productId, item.Quantity, unitPrice);        // invariants enforced by the aggregate
        }

        order.Place(time.GetUtcNow());                                  // raises OrderPlaced

        orders.Add(order);
        await unitOfWork.SaveChangesAsync(ct);                          // saves order + outbox message in ONE transaction
        return order.Id.Value;
    }
}
```

### EF Core mapping (persistence stays out of the domain)

```csharp
internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> b)
    {
        b.ToTable("Orders", schema: "ordering");
        b.HasKey(o => o.Id);
        b.Property(o => o.Id).HasConversion(id => id.Value, v => new OrderId(v));
        b.Property(o => o.CustomerId).HasConversion(id => id.Value, v => new CustomerId(v));
        b.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(o => o.Currency).HasMaxLength(3);
        b.Ignore(o => o.Total);
        b.Ignore(o => o.DomainEvents);
        b.Property<byte[]>("RowVersion").IsRowVersion();

        b.OwnsMany(o => o.Lines, line =>                 // internal entities owned by the root
        {
            line.ToTable("OrderLines", "ordering");
            line.WithOwner().HasForeignKey("OrderId");
            line.HasKey(l => l.Id);
            line.Property(l => l.Id).HasConversion(id => id.Value, v => new OrderLineId(v));
            line.Property(l => l.ProductId).HasConversion(id => id.Value, v => new ProductId(v));
            line.OwnsOne(l => l.UnitPrice, m =>
            {
                m.Property(x => x.Amount).HasColumnName("UnitPrice").HasPrecision(18, 2);
                m.Property(x => x.Currency).HasColumnName("UnitPriceCurrency").HasMaxLength(3);
            });
        });
        b.Navigation(o => o.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);   // use the private _lines field
    }
}
```

### Unit tests – the domain speaks for itself

```csharp
public class OrderTests
{
    private static readonly Money TenEuro = new(10m, "EUR");

    [Fact]
    public void PlacingAnEmptyOrder_IsNotAllowed()
    {
        var order = Order.StartFor(CustomerId.New(), "EUR");
        var act = () => order.Place(DateTimeOffset.UtcNow);
        Assert.Throws<DomainException>(act);
    }

    [Fact]
    public void AddingTheSameProductTwice_IncreasesQuantityOnOneLine()
    {
        var order = Order.StartFor(CustomerId.New(), "EUR");
        var mug = ProductId.New();

        order.AddItem(mug, 1, TenEuro);
        order.AddItem(mug, 2, TenEuro);

        var line = Assert.Single(order.Lines);
        Assert.Equal(3, line.Quantity);
        Assert.Equal(new Money(30m, "EUR"), order.Total);
    }

    [Fact]
    public void PlacingAnOrder_RaisesOrderPlaced()
    {
        var order = Order.StartFor(CustomerId.New(), "EUR");
        order.AddItem(ProductId.New(), 1, TenEuro);

        order.Place(DateTimeOffset.UtcNow);

        Assert.IsType<OrderPlaced>(Assert.Single(order.DomainEvents));
    }

    [Fact]
    public void PlacedOrders_CannotBeChanged()
    {
        var order = Order.StartFor(CustomerId.New(), "EUR");
        order.AddItem(ProductId.New(), 1, TenEuro);
        order.Place(DateTimeOffset.UtcNow);

        Assert.Throws<DomainException>(() => order.AddItem(ProductId.New(), 1, TenEuro));
    }
}
```

---

## 7. DDD with Clean Architecture, Microservices, React & Azure

### DDD + Clean Architecture

| Clean Architecture ring | DDD element |
|---|---|
| Entities (enterprise rules) | **Domain model**: aggregates, entities, value objects, domain services, domain events |
| Use cases (application rules) | **Application services** / command & query handlers |
| Interface adapters | Repository implementations, ACL adapters, API endpoints, message consumers |
| Frameworks & drivers | EF Core, ASP.NET Core, Azure SDKs, React |

They fit together naturally: Clean Architecture tells you **where** the domain sits (center, no outward dependencies); DDD tells you **how to model** what's in it, and how to split the system (Bounded Contexts).

### DDD + Microservices

- A **Bounded Context** is the best starting point for a **service boundary**.
- **Aggregates** define transaction boundaries; **domain events** become **integration events** between services.
- Don't share databases between contexts. Each owns its data.
- **Start with a modular monolith** (one deployable, one module per Bounded Context, separate DB schemas). Split a module into a service when there's a real reason (scaling, team autonomy, release cadence).

### Mapping to Azure

| DDD concept | Azure service / .NET tool |
|---|---|
| Bounded Context as a deployable | Azure **Container Apps** / **App Service** / **AKS** (one app per context, or a modular monolith on one App Service) |
| Each context owns its data | Separate **Azure SQL** databases/schemas, or **Cosmos DB** containers (partition key often = aggregate ID) |
| Aggregate concurrency | EF Core `RowVersion` (SQL) / Cosmos DB `_etag` optimistic concurrency |
| Domain/integration events | **Azure Service Bus** topics (reliable business events), **Event Grid** (reactive notifications), **Event Hubs** (high-volume streams) |
| Transactional outbox | Outbox table + background worker, or Cosmos DB **change feed** |
| Anticorruption layer to external systems | Adapters in Infrastructure; **API Management** for façades/transformations |
| Open Host Service / Published Language | Versioned REST/gRPC APIs with **OpenAPI** docs via **API Management**; event schemas in a schema registry |
| Long-running domain processes (sagas) | **Durable Functions**, or a process manager using Service Bus |
| Generic subdomains (buy, don't build) | **Entra ID / External ID** (identity), **Azure Communication Services** (email/SMS), Stripe/Adyen (payments) |

### React and DDD

- Organize the SPA by **Bounded Context / feature** too (`features/ordering`, `features/catalog`), using the same Ubiquitous Language as the backend.
- **Don't duplicate business rules** in React. The domain lives on the server; the UI may do *convenience* validation only.
- Use **task-based UIs** that map to commands ("Place order", "Request refund") rather than generic CRUD forms ("Edit order record"). Commands map directly to aggregate methods.

```tsx
// Task-based UI: buttons are domain commands, not "Save"
<button onClick={() => placeOrder.mutate({ orderId })} disabled={!order.canBePlaced}>
  Place order
</button>
<button onClick={() => requestRefund.mutate({ orderId, reason })} disabled={!order.canBeRefunded}>
  Request refund
</button>
```

---

## 8. Planning Checklist – Before You Build

**Strategic (do these first)**
- [ ] What is our **Core Domain**? Do we have a one-page **Domain Vision Statement**?
- [ ] Which subdomains are **supporting** and which are **generic** (buy/SaaS)?
- [ ] What are the **Bounded Contexts**? Is there a **Context Map** showing the relationships (Customer/Supplier, ACL, OHS, Shared Kernel…)?
- [ ] Which team owns which context? (Conway's Law: team boundaries ≈ context boundaries.)
- [ ] Which external/legacy systems need an **Anticorruption Layer**?

**Language**
- [ ] Have we run an **Event Storming** or similar workshop with domain experts?
- [ ] Is there a **glossary** per context, and does the code use exactly those words?

**Tactical**
- [ ] For each aggregate: what are its **invariants**? Is it **small**? Are other aggregates referenced **by ID**?
- [ ] Are descriptive attributes modeled as **value objects** (not primitives)?
- [ ] Is business logic on **entities/value objects** (not an anemic model with fat services)?
- [ ] One **repository per aggregate root**; read models for queries?
- [ ] Which consistency is **immediate** (same aggregate) and which is **eventual** (domain events)?
- [ ] Is event publishing **reliable** (outbox)?

**Process**
- [ ] Do developers have regular access to **domain experts**?
- [ ] Do we **refactor the model** when we learn something new, and rename code when language changes?

---

## 9. Interview Questions & Answers

**Q1. What is Domain-Driven Design?**
An approach to building complex software by focusing on the core business domain: developing a model together with domain experts, expressing it in a Ubiquitous Language used everywhere including code, and organizing large systems into Bounded Contexts with explicit relationships.

**Q2. Entity vs Value Object?**
An entity has a stable identity that persists through changes (Customer, Order). A value object has no identity, is defined by its attributes, and should be immutable (Money, Address). Equality is by ID for entities and by value for value objects.

**Q3. What is an Aggregate and why is it important?**
A cluster of objects treated as one unit for data changes, with a single root that enforces invariants. Outsiders reference only the root (by ID). It defines the transactional consistency boundary. Keep aggregates small and modify one per transaction; use domain events for cross-aggregate consistency.

**Q4. What is a Bounded Context?**
An explicit boundary within which a model and its language are consistent. Different contexts can model the same real-world thing differently (Product in Catalog vs Inventory). It's the key strategic pattern and often maps to a team and a service/module.

**Q5. What is an Anticorruption Layer?**
A translation layer (facades, adapters, translators) between your model and an external/legacy model, so the foreign concepts don't leak into and corrupt your domain.

**Q6. Domain service vs application service?**
A domain service contains business logic that doesn't belong to one entity (e.g., fund transfer). An application service orchestrates a use case (load aggregates, call domain, save, publish) and contains no business rules.

**Q7. What is the Ubiquitous Language?**
A shared vocabulary between developers and domain experts, used in conversation, documentation, and code within a Bounded Context. Changing the language means changing the model and the code.

**Q8. What is an Anemic Domain Model and why is it a problem?**
Domain classes with only data (getters/setters) and all logic in service classes. It loses the benefits of OO, scatters and duplicates rules, and lets any code put objects into invalid states.

**Q9. What are Core, Supporting, and Generic subdomains?**
Core gives competitive advantage (invest most, build in-house with the best people). Supporting is necessary and business-specific but not differentiating. Generic is a solved problem (buy or use off-the-shelf).

**Q10. What are domain events?**
Records of something meaningful that happened in the domain, named in the past tense (OrderPlaced). Used for cross-aggregate eventual consistency, integration between contexts, and audit/event sourcing.

**Q11. How do you persist DDD aggregates with EF Core without polluting the domain?**
Use Fluent API configurations in Infrastructure (no attributes on domain classes), private setters and private constructors, backing fields for collections, owned types/value converters for value objects and strongly-typed IDs, and optimistic concurrency tokens.

**Q12. How does DDD relate to microservices?**
Bounded Contexts are natural service boundaries; aggregates define transaction boundaries; domain events become integration events. But DDD doesn't require microservices. A modular monolith with clear contexts is often the better start.

**Q13. What's the Specification pattern?**
A named, combinable predicate object expressing a business rule. It's used for validation, querying (e.g., translated to SQL via expression trees), and building objects to order.

**Q14. Strategic vs tactical DDD?**
Strategic = Bounded Contexts, Context Maps, Core Domain distillation (how to split and focus). Tactical = Entities, Value Objects, Aggregates, Repositories, Factories, Services, Events (how to model inside one context). Strategic matters more; tactical patterns are optional tools.

---

## 10. When NOT to Use DDD

DDD (especially the tactical patterns) has a real cost. Evans himself says it's for **complex domains**.

- **Simple CRUD apps** (admin screens, data entry with few rules): use a simple layered or vertical-slice design. A rich model would be overkill.
- **Generic subdomains:** buy them instead of modeling them.
- **No access to domain experts:** you can't crunch knowledge alone. (Still use the Ubiquitous Language and Bounded Contexts. They're cheap and always useful.)
- **Technical/data-heavy problems** (ETL, reporting, pure integration): the "domain" is data movement; other patterns fit better.

> **Rule of thumb:** use **strategic DDD** (contexts, language, core focus) on almost every non-trivial project. Use **tactical DDD** (aggregates, rich models) where the business logic is genuinely complex, which is usually the **core domain**.

---

## 11. Key Takeaways

1. **The domain is the heart of software.** Technology is secondary.
2. **Crunch knowledge with domain experts, continuously.**
3. **Ubiquitous Language** everywhere, especially in code.
4. **One model for analysis and code.** Modelers code; coders model.
5. **Isolate the domain** from UI and infrastructure (layered / Clean Architecture).
6. **Entities** have identity; **value objects** are immutable values. Prefer value objects.
7. **Aggregates** protect invariants: small, one root, reference others by ID, one per transaction.
8. **Repositories** per aggregate root; separate read models for queries.
9. **Domain events** connect aggregates and contexts with eventual consistency.
10. **Refactor toward deeper insight.** Make implicit concepts explicit (policies, specifications).
11. **Bounded Contexts** are the most important pattern. Different models in different contexts is normal.
12. **Context Maps** and **Anticorruption Layers** manage relationships and protect your model.
13. **Focus on the Core Domain.** Buy generic, simplify supporting, invest in core.

---

*See also: [03 – Clean Architecture](../Rober%20C%20Martin/03-Clean-Architecture.md) · [07 – Functional Design](../Rober%20C%20Martin/07-Functional-Design.md)*
