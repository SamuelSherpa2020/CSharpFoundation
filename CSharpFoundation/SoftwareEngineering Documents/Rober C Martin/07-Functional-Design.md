# Functional Design: Principles, Patterns, and Practices

> **Author:** Robert C. Martin ("Uncle Bob") · **Published:** 2023
> **One-line summary:** Functional programming is **programming without assignment** (no mutation). The design principles you already know (SOLID, design patterns, Clean Architecture) **still apply** in a functional world. They just look different.
> **Book's languages:** Clojure (with Java for comparison). **This guide:** everything translated to **C# 12 / .NET 8+**, which supports a functional style well.

---

## Table of Contents

1. [What the Book Actually Covers](#1-what-the-book-actually-covers)
2. [Part I – Functional Basics](#2-part-i--functional-basics)
   - 2.1 [Immutability](#21-immutability)
   - 2.2 [Persistent Data](#22-persistent-data)
   - 2.3 [Recursion and Iteration](#23-recursion-and-iteration)
   - 2.4 [Laziness](#24-laziness)
   - 2.5 [Statefulness – When We Must Mutate](#25-statefulness--when-we-must-mutate)
3. [Part II – Comparative Analysis (OO vs FP)](#3-part-ii--comparative-analysis-oo-vs-fp)
   - 3.1 [Prime Factors](#31-prime-factors)
   - 3.2 [Bowling Game](#32-bowling-game)
   - 3.3 [Gossiping Bus Drivers](#33-gossiping-bus-drivers)
   - 3.4 [Object-Oriented Programming](#34-object-oriented-programming)
   - 3.5 [Types](#35-types)
4. [Part III – Functional Design](#4-part-iii--functional-design)
   - 4.1 [Data Flow](#41-data-flow)
   - 4.2 [SOLID in a Functional World](#42-solid-in-a-functional-world)
5. [Part IV – Functional Pragmatics](#5-part-iv--functional-pragmatics)
   - 5.1 [Tests](#51-tests)
   - 5.2 [GUI](#52-gui)
   - 5.3 [Concurrency](#53-concurrency)
6. [Part V – Design Patterns in FP](#6-part-v--design-patterns-in-fp)
7. [Functional Architecture: Functional Core, Imperative Shell](#7-functional-architecture-functional-core-imperative-shell)
8. [Applying It in .NET + React + Azure](#8-applying-it-in-net--react--azure)
9. [Interview Questions & Answers](#9-interview-questions--answers)
10. [Key Takeaways](#10-key-takeaways)

---

## 1. What the Book Actually Covers

After decades of OO, Uncle Bob spent years working in **Clojure** (a functional Lisp on the JVM). This book is what he learned. It's written for OO programmers who want to understand FP, **without** academic math (no monad tutorials).

| Part | Topic | Big idea |
|---|---|---|
| **I. Functional Basics** | Immutability, persistent data, recursion & iteration, laziness, statefulness | What FP actually *is*, and what you give up (assignment) and gain (safety). |
| **II. Comparative Analysis** | Prime Factors, Bowling Game, Gossiping Bus Drivers katas in Java *and* Clojure; then OOP and Types | Same problems solved both ways, side by side. FP and OO are not enemies. |
| **III. Functional Design** | Data flow, SOLID | Design principles survive the move to FP. |
| **IV. Functional Pragmatics** | Tests, GUI, Concurrency | How FP works in real-world concerns. |
| **V. Design Patterns** | GoF patterns reviewed in a functional style | Many patterns still apply. Some become a single function. |

**Central messages:**
1. **FP = no assignment statements.** Variables are not mutated once set.
2. **Most of what we know about design still applies.** SOLID, patterns, and architecture are about managing dependencies, not about objects vs functions.
3. **OO and FP are complementary.** "Objects" (polymorphic dispatch) work fine in FP; FP just removes mutable state.
4. **You can write functional code in any language**, including C#.

---

## 2. Part I – Functional Basics

### 2.1 Immutability

**What is functional programming?** Uncle Bob's short definition:

> **Functional programming is programming without assignment statements.**

You *bind* names to values, but you never change those values. Instead of changing data, you **create new data** from old data.

**Why bother?** Most hard bugs come from **mutable state**:
- **Race conditions:** two threads change the same variable.
- **Temporal coupling:** code works only if functions are called in the right order.
- **Spooky action at a distance:** you pass a list to a method and it gets changed behind your back.

**Real-world analogy:** A bank statement. Banks don't erase and rewrite your balance. They **append** transactions, and the balance is *derived* from them. Nobody can secretly change history.

```csharp
// ❌ Mutable: anyone holding a reference can change it
public class Address { public string City { get; set; } = ""; }

var home = new Address { City = "Pune" };
var shipping = home;
shipping.City = "Mumbai";          // 😱 home.City is now "Mumbai" too

// ✅ Immutable: records + 'with' create new values
public sealed record AddressRecord(string Street, string City, string Country);

var homeRec = new AddressRecord("MG Road 1", "Pune", "IN");
var shippingRec = homeRec with { City = "Mumbai" };   // new object; homeRec unchanged
```

**Pure functions:** same input → same output, no side effects. They are trivially testable and safe to run in parallel.

```csharp
// Pure
static decimal ApplyVat(decimal net, decimal rate) => net * (1 + rate);

// Impure (reads the clock, writes to a DB)
async Task SaveInvoiceAsync(Invoice i) { i.CreatedAt = DateTime.UtcNow; await db.SaveAsync(i); }
```

**C# toolbox for immutability:** `record` / `record struct`, `init` setters, `readonly` fields, `readonly struct`, `with` expressions, `System.Collections.Immutable`, `IReadOnlyList<T>`, `FrozenDictionary<K,V>` (.NET 8).

---

### 2.2 Persistent Data

**Question:** If I can't change a list, do I copy the whole list every time I "add" an item? That would be slow.

**Answer:** **Persistent data structures** use **structural sharing**. The new version shares almost all of its internal nodes with the old version; only the changed path is new. "Adding" to a list of a million items copies only a few small nodes.

(*Persistent* here means "old versions persist", not "saved to disk".)

```
Old list:   root ── A ── B ── C
                          \
New list:   root' ── A ── B ── C ── D     (A, B, C are shared, not copied)
```

**In C#:**

```csharp
using System.Collections.Immutable;

var v1 = ImmutableList.Create("apple", "banana");
var v2 = v1.Add("cherry");     // v1 still has 2 items; v2 has 3; they share structure
var v3 = v2.SetItem(0, "avocado");

Console.WriteLine(string.Join(",", v1)); // apple,banana
Console.WriteLine(string.Join(",", v3)); // avocado,banana,cherry

// Bulk changes efficiently with a builder (mutable inside, immutable outside)
var builder = v3.ToBuilder();
builder.AddRange(["date", "elderberry"]);
ImmutableList<string> v4 = builder.ToImmutable();
```

**Practical tip:** `ImmutableList<T>` is a tree and is slower to index than `List<T>`. For read-heavy data built once, `ImmutableArray<T>` or `FrozenSet<T>` / `FrozenDictionary<K,V>` are faster.

**Undo/redo for free:** Since old versions survive, keeping history is just keeping references.

```csharp
public sealed record Document(ImmutableList<string> Lines);

public sealed class Editor
{
    private ImmutableStack<Document> _history = ImmutableStack<Document>.Empty;
    public Document Current { get; private set; } = new([]);

    public void Apply(Func<Document, Document> change)
    {
        _history = _history.Push(Current);
        Current = change(Current);
    }

    public void Undo() => (_history, Current) = (_history.Pop(out var previous), previous);
}
```

---

### 2.3 Recursion and Iteration

Without assignment, you can't write `i++` in a loop. FP languages loop with **recursion** instead.

```csharp
// Iterative (mutates 'total' and 'i')
static int SumLoop(int[] xs) { var total = 0; for (var i = 0; i < xs.Length; i++) total += xs[i]; return total; }

// Recursive (no mutation)
static int SumRec(ReadOnlySpan<int> xs) => xs.IsEmpty ? 0 : xs[0] + SumRec(xs[1..]);
```

**Tail-call optimization (TCO):** If the recursive call is the *last* thing a function does, a compiler can reuse the stack frame, turning recursion into a loop. Clojure does this explicitly with `loop/recur`.

**C# warning:** the C# compiler **does not guarantee** TCO. Deep recursion (say 100,000 levels) can throw `StackOverflowException`. So in C#:
- Use recursion for naturally recursive data (trees, ASTs, small lists).
- For long sequences, use **LINQ** (`Sum`, `Aggregate`, `Select`), which keeps the mutation hidden inside the library.

```csharp
// Functional *and* stack-safe in C#
static int Sum(IEnumerable<int> xs) => xs.Aggregate(0, (acc, x) => acc + x);

// Recursion is natural for trees
public abstract record Expr;
public sealed record Num(decimal Value) : Expr;
public sealed record Add(Expr Left, Expr Right) : Expr;
public sealed record Mul(Expr Left, Expr Right) : Expr;

static decimal Eval(Expr e) => e switch
{
    Num n => n.Value,
    Add a => Eval(a.Left) + Eval(a.Right),
    Mul m => Eval(m.Left) * Eval(m.Right),
    _ => throw new NotSupportedException()
};

// (2 + 3) * 4 = 20
Console.WriteLine(Eval(new Mul(new Add(new Num(2), new Num(3)), new Num(4))));
```

---

### 2.4 Laziness

A **lazy sequence** computes its elements only when someone asks for them. This lets you describe **infinite** sequences and process huge data without loading it all.

**Real-world analogy:** A streaming video. You don't download the whole film before watching. Each chunk arrives when needed.

C# has had laziness since 2005: `IEnumerable<T>` + `yield return`, and LINQ operators are lazy.

```csharp
// An infinite sequence of primes – computed only as far as needed
static IEnumerable<int> Primes()
{
    for (var n = 2; ; n++)
        if (IsPrime(n)) yield return n;
}
static bool IsPrime(int n) => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(d => n % d != 0);

var firstTen = Primes().Take(10).ToList();   // only 10 primes are ever computed

// Lazy file processing: handles a 10 GB log without loading it into memory
var errors = File.ReadLines("app.log")
                 .Where(line => line.Contains("ERROR"))
                 .Select(ParseLogEntry)
                 .Take(100);
```

**Gotchas:**
- **Multiple enumeration:** a lazy query re-runs every time you enumerate it. Call `.ToList()` once if you need the results repeatedly.
- **Side effects inside lazy pipelines** run at surprising times. Keep pipelines pure.
- Async laziness: `IAsyncEnumerable<T>` with `await foreach` (great for paging through Cosmos DB or streaming API results).

---

### 2.5 Statefulness – When We Must Mutate

Real programs **must** change state: a database row changes, a counter increments, the screen updates. FP doesn't pretend otherwise. The rule is:

> **Keep mutation rare, explicit, and isolated, and make it safe.**

Clojure provides an **atom**: one reference to an immutable value. You update it with `swap!`, giving it a pure function. If another thread changed the atom in the meantime, `swap!` retries. The book also covers **Software Transactional Memory (STM)**, where several references change together in a transaction.

**C# equivalent of an atom:** `ImmutableInterlocked.Update` (compare-and-swap with retry).

```csharp
public sealed record Inventory(ImmutableDictionary<string, int> Stock)
{
    public Inventory Remove(string sku, int qty) =>
        Stock.GetValueOrDefault(sku) >= qty
            ? this with { Stock = Stock.SetItem(sku, Stock[sku] - qty) }
            : throw new InvalidOperationException($"Not enough {sku}");
}

public sealed class InventoryAtom
{
    private Inventory _current = new(ImmutableDictionary<string, int>.Empty.Add("MUG", 10));
    public Inventory Current => Volatile.Read(ref _current);

    // Like Clojure's swap!: apply a pure function atomically; retries if another thread got there first.
    public Inventory Swap(Func<Inventory, Inventory> f)
    {
        ImmutableInterlocked.Update(ref _current, f);
        return Current;
    }
}

// Safe from 100 threads at once, no locks in business code
var atom = new InventoryAtom();
Parallel.For(0, 5, _ => atom.Swap(inv => inv.Remove("MUG", 1)));
Console.WriteLine(atom.Current.Stock["MUG"]);   // 5
```

> Note: the update function may run more than once under contention, so it **must be pure** (no logging, emails, or DB writes inside it).

**Other places to "allow" mutation:** the database, a cache, the UI. In architecture terms, mutation lives at the **edges** (see section 7).

---

## 3. Part II – Comparative Analysis (OO vs FP)

The book solves the same katas in Java (OO/procedural) and Clojure (functional). Here they are in **functional C#**. Compare them with the imperative versions in [05 – Agile PPP](05-Agile-Software-Development-PPP.md).

### 3.1 Prime Factors

```csharp
public static class PrimeFactors
{
    public static ImmutableList<int> Of(int n) => Of(n, 2, ImmutableList<int>.Empty);

    private static ImmutableList<int> Of(int n, int divisor, ImmutableList<int> factors) =>
        n <= 1                ? factors
        : n % divisor == 0    ? Of(n / divisor, divisor, factors.Add(divisor))
        :                       Of(n, divisor + 1, factors);
}

// PrimeFactors.Of(360) → [2, 2, 2, 3, 3, 5]
```

No loop counters, no mutation. Each call returns a **new** list. (Depth is bounded by the number of divisors tried, so for very large primes prefer a loop.)

### 3.2 Bowling Game

The OO version keeps a `rolls` array and a mutable index. The functional version treats the game as a **transformation of a list of rolls into a list of frame scores**:

```csharp
public static class Bowling
{
    public static int Score(IEnumerable<int> rolls) => Frames(rolls.ToArray()).Take(10).Sum();

    // Lazily turns rolls into frame scores using list patterns (C# 11+)
    private static IEnumerable<int> Frames(int[] rolls) => rolls switch
    {
        [10, var a, var b, ..]                => Frames(rolls[1..]).Prepend(10 + a + b),   // strike
        [var a, var b, var c, ..] when a + b == 10 => Frames(rolls[2..]).Prepend(10 + c),  // spare
        [var a, var b, ..]                    => Frames(rolls[2..]).Prepend(a + b),         // open frame
        [var a]                               => [a],
        []                                    => []
    };
}

// Bowling.Score(Enumerable.Repeat(10, 12)) → 300
```

Notice how **laziness** (`Take(10)`) neatly handles the "10th frame bonus balls" problem: extra frames are never computed or counted.

### 3.3 Gossiping Bus Drivers

**The kata:** Each bus driver drives a circular route of stops and knows one piece of gossip at the start. Each minute every driver is at a stop; drivers at the same stop share **all** the gossip they know. How many minutes until every driver knows every piece of gossip? If it takes more than 480 minutes (one working day), answer "never".

**Functional approach:** the "world" is an immutable value. Each minute is a pure function `World → World`.

```csharp
public sealed record Driver(string Name, ImmutableArray<int> Route, ImmutableHashSet<string> Rumors, int Position = 0)
{
    public int Stop => Route[Position % Route.Length];
    public Driver Drive() => this with { Position = Position + 1 };
}

public static class Gossip
{
    private const int WorkingDayMinutes = 480;

    public static int? MinutesUntilAllKnowEverything(IReadOnlyList<Driver> drivers)
    {
        var allRumors = drivers.SelectMany(d => d.Rumors).ToImmutableHashSet();
        var world = drivers;

        for (var minute = 1; minute <= WorkingDayMinutes; minute++)
        {
            world = Share(world);
            if (world.All(d => d.Rumors.SetEquals(allRumors))) return minute;
            world = world.Select(d => d.Drive()).ToList();
        }
        return null;   // "never"
    }

    // Pure: every driver learns everything known by drivers at the same stop
    private static IReadOnlyList<Driver> Share(IReadOnlyList<Driver> drivers) =>
        drivers.Select(d => d with
        {
            Rumors = drivers.Where(other => other.Stop == d.Stop)
                            .SelectMany(other => other.Rumors)
                            .ToImmutableHashSet()
        }).ToList();
}

// Usage
var drivers = new[]
{
    new Driver("A", [3, 1, 2, 3], ["a"]),
    new Driver("B", [3, 2, 3, 1], ["b"]),
    new Driver("C", [4, 2, 3, 4, 5], ["c"]),
};
Console.WriteLine(Gossip.MinutesUntilAllKnowEverything(drivers)?.ToString() ?? "never");
```

The book's comparison: the Java version needed classes with mutable state that changed during the loop. The Clojure version is a small pipeline of pure transformations, which is easier to test and reason about.

### 3.4 Object-Oriented Programming

**Uncle Bob's view:** OO is not the opposite of FP.
- The essential part of OO is **polymorphism** (dynamic dispatch), which gives dependency inversion.
- FP languages have polymorphism too (Clojure's multimethods and protocols).
- What FP removes is **mutable state inside objects**, not objects themselves.

So a **functional object** is an immutable value plus functions that return new values. C# records are exactly that:

```csharp
// "Functional OO": polymorphism + immutability
public interface IShape { double Area { get; } IShape Scale(double factor); }

public sealed record Circle(double Radius) : IShape
{
    public double Area => Math.PI * Radius * Radius;
    public IShape Scale(double f) => this with { Radius = Radius * f };   // new object, no mutation
}

public sealed record Square(double Side) : IShape
{
    public double Area => Side * Side;
    public IShape Scale(double f) => this with { Side = Side * f };
}
```

**Closures are objects:** a function that captures variables is equivalent to an object with fields and one method.

```csharp
// "Object" built from a closure
static Func<decimal, decimal> MakeTaxCalculator(decimal rate) => net => net * (1 + rate);

var germanVat = MakeTaxCalculator(0.19m);
var indianGst = MakeTaxCalculator(0.18m);
Console.WriteLine(germanVat(100m));   // 119
```

### 3.5 Types

Clojure is **dynamically typed**. Uncle Bob describes the trade-off:
- **Static types** catch some errors at compile time and help tools (autocomplete, refactoring).
- **Dynamic types** are more flexible and less verbose. He relies on **tests** (and on Clojure's `spec` library for runtime data validation) instead of a compiler.
- His view: **tests are the more important safety net** either way. Types don't prove behavior is correct.

**In C#** you get static types. Use them functionally to **make illegal states unrepresentable**:

```csharp
// ❌ Stringly-typed and nullable chaos
public class Payment { public string Status; public string? CardLast4; public string? FailureReason; }

// ✅ A closed hierarchy (a discriminated union): each state carries exactly its own data
public abstract record PaymentState
{
    private PaymentState() { }
    public sealed record Pending : PaymentState;
    public sealed record Authorized(string CardLast4, string AuthCode) : PaymentState;
    public sealed record Failed(string Reason) : PaymentState;
}

static string Describe(PaymentState s) => s switch
{
    PaymentState.Pending                 => "Waiting…",
    PaymentState.Authorized(var last4, _) => $"Paid with card ending {last4}",
    PaymentState.Failed(var reason)      => $"Failed: {reason}",
    _ => throw new UnreachableException()
};
```

Also: **value objects** instead of primitives (`record struct Email(string Value)`) and validation at the edges (FluentValidation / `System.ComponentModel.DataAnnotations`), similar to Clojure `spec`.

---

## 4. Part III – Functional Design

### 4.1 Data Flow

In OO, we often design **objects that talk to each other** and change state. In FP, we design **data flowing through a pipeline of transformations**.

**Real-world analogy:** A factory assembly line. Raw material goes in at one end; each station does one transformation; the finished product comes out. Stations don't reach back to change previous stations.

```csharp
// A data-flow design for an order-pricing pipeline. Every step is a pure function.
public sealed record PricedOrder(Order Order, decimal Subtotal, decimal Discount, decimal Tax, decimal Total);

public static class Pricing
{
    public static PricedOrder Price(Order order, PricingRules rules) =>
        order
            .Pipe(o => new PricedOrder(o, Subtotal(o), 0, 0, 0))
            .Pipe(p => ApplyDiscounts(p, rules.Discounts))
            .Pipe(p => ApplyTax(p, rules.TaxRate))
            .Pipe(p => p with { Total = p.Subtotal - p.Discount + p.Tax });

    private static decimal Subtotal(Order o) => o.Lines.Sum(l => l.UnitPrice * l.Quantity);

    private static PricedOrder ApplyDiscounts(PricedOrder p, IEnumerable<Func<PricedOrder, decimal>> discounts) =>
        p with { Discount = discounts.Sum(d => d(p)) };

    private static PricedOrder ApplyTax(PricedOrder p, decimal rate) =>
        p with { Tax = Math.Round((p.Subtotal - p.Discount) * rate, 2) };
}

// Tiny helper so C# reads like a pipeline (F#'s |>)
public static class FunctionalExtensions
{
    public static TOut Pipe<TIn, TOut>(this TIn input, Func<TIn, TOut> f) => f(input);
}
```

**LINQ is the C# data-flow language:** `Where` (filter), `Select` (map), `Aggregate` (reduce), `GroupBy`, `Zip`, `Chunk`.

### 4.2 SOLID in a Functional World

Uncle Bob's key claim: **SOLID is not an OO thing.** It's about managing dependencies and reasons to change. In FP:

| Principle | OO form | Functional form | C# example |
|---|---|---|---|
| **SRP** | A class answers to one actor | A **module/namespace** of functions answers to one actor | `static class PayrollCalculations` separate from `static class PayrollReports` |
| **OCP** | Add a subclass | Pass a **new function**, or add a new case to an open dispatch (multimethod/protocol) | `IEnumerable<Func<PricedOrder, decimal>> discounts` |
| **LSP** | Subclasses honor the base contract | Any function passed in honors the **expected contract** (types + behavior) | Every discount function returns a non-negative amount |
| **ISP** | Small interfaces | Functions take **only the data they need**, not a huge map/object | `Func<decimal, decimal>` instead of `IEverythingService` |
| **DIP** | Depend on interfaces | Depend on **function signatures**; high-level code receives low-level functions as arguments | `Func<CustomerId, Task<Customer>> loadCustomer` |

```csharp
// DIP the functional way: the policy receives its dependencies as functions
public static class Reminders
{
    public static async Task SendOverdueRemindersAsync(
        Func<DateOnly, Task<IReadOnlyList<Invoice>>> findOverdue,   // ← low-level detail, injected
        Func<Invoice, Task> sendReminder,                          // ← low-level detail, injected
        DateOnly today)
    {
        foreach (var invoice in await findOverdue(today))
            await sendReminder(invoice);
    }
}

// Test without any mocking framework:
var sent = new List<Invoice>();
await Reminders.SendOverdueRemindersAsync(
    _ => Task.FromResult<IReadOnlyList<Invoice>>([Invoice.Sample()]),
    i => { sent.Add(i); return Task.CompletedTask; },
    new DateOnly(2026, 11, 19));
Assert.Single(sent);
```

> In larger C# apps, interfaces + DI are usually clearer than passing many delegates. Use delegates for small, single-method dependencies (strategies, policies, clocks).

---

## 5. Part IV – Functional Pragmatics

### 5.1 Tests

- **TDD works the same in FP**, and it's easier: pure functions need no setup, no mocks, no cleanup.
- Clojure programmers use the **REPL** heavily. Uncle Bob still writes tests, because REPL experiments disappear while tests stay.
- **Property-based testing** fits FP well: instead of examples, state a **property** that must hold for *all* inputs, and let the tool generate hundreds of random cases.

```csharp
// FsCheck.Xunit – property-based tests in C#
public class PrimeFactorsProperties
{
    [Property]
    public bool ProductOfFactors_EqualsInput(PositiveInt n) =>
        PrimeFactors.Of(n.Get).Aggregate(1, (a, b) => a * b) == n.Get;   // empty product = 1 covers n = 1

    [Property]
    public bool AllFactorsArePrime(PositiveInt n) =>
        PrimeFactors.Of(n.Get).All(f => Enumerable.Range(2, Math.Max(0, (int)Math.Sqrt(f) - 1)).All(d => f % d != 0));
}
```

### 5.2 GUI

The book builds a GUI (turtle graphics with the Quil library) the functional way:
- The whole UI state is **one immutable value**.
- An **update** function takes `(state, event) → new state`. It's pure and easy to test.
- A **draw** function takes `state → screen`. It's a humble object.

If you know React, this is **exactly** the `useReducer` / Redux / Elm architecture:

```tsx
// React – pure reducer (testable without rendering) + humble view
type Cart = { items: { sku: string; qty: number }[] };
type Action = { type: "add"; sku: string } | { type: "remove"; sku: string };

export function cartReducer(state: Cart, action: Action): Cart {
  switch (action.type) {
    case "add": {
      const existing = state.items.find(i => i.sku === action.sku);
      return existing
        ? { items: state.items.map(i => i.sku === action.sku ? { ...i, qty: i.qty + 1 } : i) }
        : { items: [...state.items, { sku: action.sku, qty: 1 }] };
    }
    case "remove":
      return { items: state.items.filter(i => i.sku !== action.sku) };
  }
}
```

The same pattern in C# (for Blazor, MAUI, or a game loop):

```csharp
public sealed record CounterState(int Count);
public abstract record CounterEvent { public sealed record Increment : CounterEvent; public sealed record Reset : CounterEvent; }

public static CounterState Update(CounterState s, CounterEvent e) => e switch
{
    CounterEvent.Increment => s with { Count = s.Count + 1 },
    CounterEvent.Reset     => s with { Count = 0 },
    _ => s
};
```

### 5.3 Concurrency

- **Immutable data can be shared between threads with no locks.** Nobody can change it.
- Shared mutable state goes into a few controlled references (atoms, agents, STM).
- **Message passing** (agents, channels, actors) avoids shared state altogether.

```csharp
// Fan-out over immutable inputs – no locks needed
ImmutableArray<Order> orders = await LoadOrdersAsync();
var priced = await Task.WhenAll(orders.Select(o => Task.Run(() => Pricing.Price(o, rules))));

// Message passing with Channels (similar to Clojure core.async)
var channel = Channel.CreateUnbounded<OrderPlaced>();
_ = Task.Run(async () =>
{
    await foreach (var evt in channel.Reader.ReadAllAsync())
        await HandleAsync(evt);           // single consumer → no shared-state races
});
await channel.Writer.WriteAsync(new OrderPlaced(orderId));
```

At Azure scale: **Service Bus** queues, **Event Hubs**, **Durable Functions**, and **Orleans** actors are the distributed versions of the same idea: share messages, not memory.

---

## 6. Part V – Design Patterns in FP

The book reviews classic GoF patterns and shows that **most still apply** in FP. They often become much smaller, because a "one-method interface" is just a function.

| Pattern | OO shape | Functional shape (C#) |
|---|---|---|
| **Strategy** | Interface + classes | Pass a `Func<…>` |
| **Command** | `ICommand.Execute()` | A function value, or an immutable record describing the action (data as command) |
| **Abstract Server** | Client depends on an interface | Client depends on a function signature / protocol |
| **Adapter** | Class wrapping another | A function that converts one signature to another |
| **Decorator** | Wrapper class | **Function composition**: `f => x => { log(x); return f(x); }` |
| **Composite** | Tree of objects with a common interface | Recursive data + recursive function (`Expr` / `Eval` above) |
| **Visitor** | Double dispatch | **Pattern matching** over a closed hierarchy |
| **Template Method** | Abstract base class | **Higher-order function** taking the varying steps as parameters |
| **Abstract Factory** | Factory interface | A function that returns functions/values |
| **Observer** | Subscriber list | Event streams (Rx.NET `IObservable<T>`), callbacks |

```csharp
// Decorator = function composition
static Func<T, TResult> WithLogging<T, TResult>(Func<T, TResult> f, ILogger log, string name) =>
    x =>
    {
        log.LogInformation("Calling {Name} with {Input}", name, x);
        var result = f(x);
        log.LogInformation("{Name} returned {Result}", name, result);
        return result;
    };

static Func<T, TResult> WithCache<T, TResult>(Func<T, TResult> f) where T : notnull
{
    var cache = new ConcurrentDictionary<T, TResult>();
    return x => cache.GetOrAdd(x, f);
}

Func<string, decimal> getPrice = sku => catalog.LookupPrice(sku);
var decorated = WithLogging(WithCache(getPrice), logger, "GetPrice");

// Template Method = higher-order function
static async Task<ImportResult> Import<TRow>(
    Stream input,
    Func<Stream, IAsyncEnumerable<TRow>> parse,     // varying step
    Func<TRow, bool> validate,                      // varying step
    Func<IReadOnlyList<TRow>, Task> save)           // varying step
{
    var valid = new List<TRow>();
    await foreach (var row in parse(input))
        if (validate(row)) valid.Add(row);
    await save(valid);
    return new ImportResult(valid.Count);
}
```

**Uncle Bob's point:** patterns are names for **dependency structures**. The structure (who depends on what, where polymorphism is used) is the same in OO and FP. Only the syntax changes.

---

## 7. Functional Architecture: Functional Core, Imperative Shell

Uncle Bob's position is that **architecture doesn't change** in FP. The Clean Architecture Dependency Rule still applies: business rules in the middle, details at the edges.

A practical way to combine Clean Architecture with FP in C# is the **"functional core, imperative shell"** pattern:

```
┌──────────────────── Imperative Shell (impure) ────────────────────┐
│  ASP.NET endpoints · EF Core · Azure SDKs · clock · random · logs │
│                                                                   │
│   1. Read data (I/O)  ──▶  ┌─── Functional Core (pure) ───┐       │
│                            │ records · pure functions ·    │       │
│   2. Call pure core   ──▶  │ decisions · calculations      │       │
│                            └───────────────────────────────┘       │
│   3. Write results / side effects (I/O) based on the decision     │
└───────────────────────────────────────────────────────────────────┘
```

```csharp
// --- Functional core: pure, no I/O, trivially testable ---
public abstract record RenewalDecision
{
    public sealed record Renew(decimal NewPremium) : RenewalDecision;
    public sealed record Decline(string Reason) : RenewalDecision;
}

public static class RenewalRules
{
    public static RenewalDecision Decide(Policy policy, IReadOnlyList<Claim> claims, DateOnly today) =>
        claims.Count(c => c.Date > today.AddYears(-3)) switch
        {
            > 3 => new RenewalDecision.Decline("Too many claims in 3 years"),
            var n => new RenewalDecision.Renew(policy.Premium * (1 + 0.1m * n))
        };
}

// --- Imperative shell: does I/O, then delegates decisions to the core ---
public sealed class RenewPolicyHandler(IPolicyRepository policies, IClaimRepository claims, IClock clock, IMailer mailer)
{
    public async Task Handle(Guid policyId, CancellationToken ct)
    {
        var policy = await policies.GetAsync(policyId, ct);                        // I/O
        var history = await claims.ForPolicyAsync(policyId, ct);                   // I/O

        var decision = RenewalRules.Decide(policy, history, clock.Today);          // PURE

        switch (decision)                                                          // I/O based on decision
        {
            case RenewalDecision.Renew r:
                await policies.SaveAsync(policy with { Premium = r.NewPremium }, ct);
                await mailer.SendRenewalAsync(policy, r.NewPremium, ct);
                break;
            case RenewalDecision.Decline d:
                await mailer.SendDeclineAsync(policy, d.Reason, ct);
                break;
        }
    }
}
```

Most tests target `RenewalRules.Decide`: no mocks, millisecond-fast, and easy to property-test.

---

## 8. Applying It in .NET + React + Azure

| Area | Functional practice |
|---|---|
| **Domain model** | Records for value objects and events; pure domain functions; closed hierarchies for states |
| **Application layer** | Functional core / imperative shell; handlers do I/O, then call pure logic |
| **Collections** | LINQ pipelines; `IReadOnlyList<T>` in signatures; `ImmutableArray<T>` / `FrozenDictionary<K,V>` for shared data |
| **Concurrency** | Immutable messages; `Channel<T>`; `ImmutableInterlocked` for rare shared state |
| **Event sourcing** | Events are immutable facts; state = `events.Aggregate(initial, Apply)`, a pure fold |
| **React** | Pure components, `useReducer`/Redux reducers, never mutate state (`[...arr]`, `{...obj}`), TanStack Query for server state |
| **Azure** | Durable Functions orchestrators must be **deterministic** (FP discipline required); Event Grid / Service Bus carry immutable events; Cosmos DB change feed as a stream |
| **F#** | If you like this style, F# runs on .NET and interoperates fully with C#. It's useful for domain cores, scripts, and data processing |

```csharp
// Event sourcing = a pure left fold
public abstract record AccountEvent;
public sealed record Deposited(decimal Amount) : AccountEvent;
public sealed record Withdrawn(decimal Amount) : AccountEvent;

public sealed record Account(decimal Balance)
{
    public static Account Empty { get; } = new(0);
    public Account Apply(AccountEvent e) => e switch
    {
        Deposited d => this with { Balance = Balance + d.Amount },
        Withdrawn w => this with { Balance = Balance - w.Amount },
        _ => this
    };
}

var state = events.Aggregate(Account.Empty, (acc, e) => acc.Apply(e));
```

---

## 9. Interview Questions & Answers

**Q1. What is functional programming?**
Programming without mutation (no assignment after binding). You build programs from pure functions that transform immutable values into new values, and push side effects to the edges.

**Q2. What's a pure function and why does it matter?**
Same input always gives the same output, with no side effects. Pure functions are easy to test, cache, parallelize, and reason about.

**Q3. What are persistent data structures?**
Immutable collections where "modified" versions share most of their structure with the original, so updates are cheap and old versions stay valid. In .NET: `System.Collections.Immutable`.

**Q4. Does C# support tail-call optimization?**
Not guaranteed by the C# compiler. Use recursion for bounded or tree-shaped data, and LINQ or loops for long sequences.

**Q5. How do you handle state in FP?**
Keep state as immutable values and compute new versions. When shared mutable state is unavoidable, isolate it in a few references updated atomically with pure functions (Clojure atom / `ImmutableInterlocked.Update`), or use message passing.

**Q6. Do SOLID principles apply to functional programming?**
Yes. They're about dependencies and reasons to change. SRP becomes cohesive modules; OCP comes from higher-order functions and polymorphic dispatch; DIP means depending on function signatures/protocols rather than concrete implementations.

**Q7. Are OO and FP opposites?**
No. OO's core value is polymorphism (dependency inversion); FP's core value is immutability. You can have both: immutable objects with polymorphic behavior, like C# records implementing interfaces.

**Q8. What is "functional core, imperative shell"?**
Put all decision-making logic in pure functions (the core) and keep I/O in a thin outer layer (the shell) that gathers inputs, calls the core, and acts on its result.

**Q9. How do design patterns change in FP?**
Many one-method patterns become plain functions (Strategy, Command). Decorator becomes function composition, Visitor becomes pattern matching, and Template Method becomes a higher-order function. The dependency structure stays the same.

**Q10. What's property-based testing?**
Instead of hand-picked examples, you state properties that must hold for all inputs (e.g., "the product of the prime factors equals the input"), and a tool like FsCheck generates many random cases to try to break them.

**Q11. How does React relate to functional programming?**
Components are (ideally) pure functions of props/state; state updates must be immutable; reducers are pure `(state, action) → state` functions. It's the same update/draw split the book uses for its GUI chapter.

---

## 10. Key Takeaways

1. **FP = no assignment.** Create new values; don't change old ones.
2. **Immutability removes whole classes of bugs**: races, temporal coupling, aliasing.
3. **Persistent data structures** make immutability efficient (structural sharing).
4. **Recursion replaces loops** in FP. In C#, prefer LINQ for long sequences (no guaranteed TCO).
5. **Laziness** handles infinite and huge data. `IEnumerable` + `yield` is C#'s lazy sequence.
6. **Mutate rarely, explicitly, atomically**, and at the edges.
7. **OO and FP are complementary**: polymorphism plus immutability.
8. **SOLID, patterns, and Clean Architecture still apply.** They're about dependencies, not paradigms.
9. **Design as data flow**: pipelines of pure transformations.
10. **Functional core, imperative shell** is the practical architecture.
11. **Tests are still the safety net**, and FP makes them easier (add property-based testing).

---

*Previous: [06 – Clean Craftsmanship](06-Clean-Craftsmanship.md) · Next: [08 – We, Programmers](08-We-Programmers.md)*
