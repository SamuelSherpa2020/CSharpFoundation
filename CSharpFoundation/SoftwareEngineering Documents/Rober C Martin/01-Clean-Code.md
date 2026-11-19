# Clean Code: A Handbook of Agile Software Craftsmanship

> **Author:** Robert C. Martin ("Uncle Bob") · **Published:** 2008
> **One-line summary:** Code is read far more often than it is written, so write it for the *next human* who reads it.
> **Examples in this guide:** C# 12 / .NET 8+ (works the same on .NET 9/10).

---

## Table of Contents

1. [What the Book Actually Covers](#1-what-the-book-actually-covers)
2. [Core Teachings with C# Examples](#2-core-teachings-with-c-examples)
   - 2.1 [Meaningful Names](#21-meaningful-names)
   - 2.2 [Functions](#22-functions)
   - 2.3 [Comments](#23-comments)
   - 2.4 [Formatting](#24-formatting)
   - 2.5 [Objects vs Data Structures](#25-objects-vs-data-structures)
   - 2.6 [Error Handling](#26-error-handling)
   - 2.7 [Boundaries (Third-Party Code)](#27-boundaries-third-party-code)
   - 2.8 [Unit Tests](#28-unit-tests)
   - 2.9 [Classes](#29-classes)
   - 2.10 [Systems](#210-systems)
   - 2.11 [Emergent Design – 4 Rules of Simple Design](#211-emergent-design--4-rules-of-simple-design)
   - 2.12 [Concurrency](#212-concurrency)
   - 2.13 [Successive Refinement](#213-successive-refinement)
   - 2.14 [Code Smells & Heuristics](#214-code-smells--heuristics)
3. [Applying It as a .NET + React + Azure Developer](#3-applying-it-as-a-net--react--azure-developer)
4. [Pull-Request Checklist](#4-pull-request-checklist)
5. [Interview Questions & Answers](#5-interview-questions--answers)
6. [Modern Nuance – Where Not to Over-Apply](#6-modern-nuance--where-not-to-over-apply)
7. [Key Takeaways](#7-key-takeaways)

---

## 1. What the Book Actually Covers

The book has three parts:

| Part | Chapters | What it's about |
|---|---|---|
| **Part 1 – Principles** | 1–13 | The rules: names, functions, comments, formatting, objects, errors, boundaries, tests, classes, systems, emergence, concurrency. |
| **Part 2 – Case Studies** | 14–16 | Uncle Bob takes real, messy Java code (a command-line argument parser, JUnit internals, a date class) and cleans it step by step. The lesson: clean code is *achieved by refactoring*, not written perfectly on the first try. |
| **Part 3 – Smells & Heuristics** | 17 | A catalogue of ~65 "smells" (signs of bad code) you can use as a review checklist. |

**The central idea:**

> The only valid measurement of code quality is *WTFs per minute* during a code review.

Bad code slows teams down over time. Every shortcut adds a little mess; the mess grows until productivity approaches zero and someone proposes "the Grand Redesign," which usually fails too. The only way to go fast is to **keep the code clean all the time.**

**The Boy Scout Rule:** *Leave the code a little cleaner than you found it.* Rename one variable, extract one method, delete one dead line — every time you touch a file.

---

## 2. Core Teachings with C# Examples

### 2.1 Meaningful Names

**Rule:** A name should tell you *why it exists, what it does, and how it's used*. If you need a comment to explain a name, the name is wrong.

**Real-world analogy:** A labelled spice rack vs. 20 identical unlabelled jars. Both "work," but one makes you open every jar.

#### ❌ Bad

```csharp
public List<int[]> Get(List<int[]> l)
{
    var l1 = new List<int[]>();
    foreach (var x in l)
        if (x[0] == 4)
            l1.Add(x);
    return l1;
}
```

What is `4`? What is `x[0]`? Nobody knows.

#### ✅ Good

```csharp
public enum CellStatus { Empty = 0, Flagged = 4 }

public record Cell(CellStatus Status, int Row, int Column)
{
    public bool IsFlagged => Status == CellStatus.Flagged;
}

public IReadOnlyList<Cell> GetFlaggedCells(IEnumerable<Cell> gameBoard) =>
    gameBoard.Where(cell => cell.IsFlagged).ToList();
```

#### Naming rules cheat sheet

| Rule | Bad | Good |
|---|---|---|
| Intention-revealing | `int d; // days` | `int daysSinceLastLogin` |
| Avoid disinformation | `accountList` (when it's a `HashSet`) | `accounts` |
| Meaningful distinctions | `ProductInfo` vs `ProductData` | `Product` vs `ProductPriceHistory` |
| Pronounceable | `genymdhms` | `generationTimestamp` |
| Searchable | `if (s > 7)` | `const int MaxLoginAttempts = 7;` |
| No encodings / Hungarian | `strName`, `iCount`, `m_total` | `name`, `count`, `_total` (C# field convention is OK) |
| Classes = **nouns** | `Process`, `Manage` | `Invoice`, `PaymentGateway` |
| Methods = **verbs** | `Invoice()` | `SendInvoice()`, `IsPaid()` |
| One word per concept | `Fetch`, `Retrieve`, `Get` mixed | Pick `Get` everywhere |
| Use domain names | `Thing`, `Item`, `Data` | `Policy`, `Claim`, `Premium` |

> **C# tip:** Follow Microsoft conventions — `PascalCase` for types/methods/properties, `camelCase` for locals/parameters, `_camelCase` for private fields, `I` prefix for interfaces (`IOrderRepository`) is the accepted .NET exception to "no encodings."

---

### 2.2 Functions

**Rules:**
1. **Small.** Then smaller. Most functions should fit on a screen; many should be < 20 lines.
2. **Do one thing.** If you can extract another meaningful function from it, it was doing more than one thing.
3. **One level of abstraction per function.** Don't mix "business decision" with "string concatenation."
4. **Few arguments.** 0 is ideal, 1–2 fine, 3 suspicious, 4+ needs a parameter object.
5. **No flag (bool) arguments.** A `bool` parameter announces the function does two things.
6. **No side effects.** A function named `CheckPassword` should not also start a session.
7. **Command–Query Separation (CQS).** A function either *does* something or *answers* something, not both.
8. **Prefer exceptions to error codes.**
9. **DRY** – Don't Repeat Yourself.

**Real-world analogy:** A restaurant. The waiter takes orders, the chef cooks, the cashier bills. If one person does all three, it works for 2 tables and collapses at 20.

#### ❌ Bad – one function doing everything

```csharp
public async Task<string> ProcessOrder(Order order, bool sendEmail)
{
    if (order == null) return "ERR1";
    if (order.Items.Count == 0) return "ERR2";

    decimal total = 0;
    foreach (var i in order.Items)
    {
        total += i.Price * i.Qty;
        if (i.Qty > 10) total -= i.Price * i.Qty * 0.05m;
    }
    if (order.Customer.IsVip) total *= 0.9m;

    using var conn = new SqlConnection("Server=...;");
    await conn.OpenAsync();
    var cmd = new SqlCommand($"INSERT INTO Orders VALUES ({order.Id}, {total})", conn); // SQL injection too!
    await cmd.ExecuteNonQueryAsync();

    if (sendEmail)
    {
        var smtp = new SmtpClient("smtp.company.com");
        await smtp.SendMailAsync("shop@co.com", order.Customer.Email, "Order", $"Total {total}");
    }
    return "OK";
}
```

Problems: validation + pricing + persistence + notification, error codes, a flag argument, hard-coded infrastructure.

#### ✅ Good – each function does one thing, at one level of abstraction

```csharp
public sealed class PlaceOrderHandler(
    IPricingService pricing,
    IOrderRepository orders,
    IOrderNotifier notifier)
{
    public async Task<OrderId> Handle(Order order, CancellationToken ct)
    {
        OrderValidator.EnsureValid(order);           // throws if invalid
        order.ApplyTotal(pricing.CalculateTotal(order));
        await orders.AddAsync(order, ct);
        await notifier.OrderPlacedAsync(order, ct);
        return order.Id;
    }
}

public sealed class PricingService : IPricingService
{
    private const int BulkQuantityThreshold = 10;
    private const decimal BulkDiscount = 0.05m;
    private const decimal VipDiscount = 0.10m;

    public decimal CalculateTotal(Order order)
    {
        var subtotal = order.Items.Sum(LineTotal);
        return order.Customer.IsVip ? ApplyDiscount(subtotal, VipDiscount) : subtotal;
    }

    private static decimal LineTotal(OrderItem item)
    {
        var gross = item.Price * item.Quantity;
        return item.Quantity > BulkQuantityThreshold ? ApplyDiscount(gross, BulkDiscount) : gross;
    }

    private static decimal ApplyDiscount(decimal amount, decimal rate) => amount * (1 - rate);
}
```

Read `Handle` top to bottom: it reads like a paragraph of English. That's the **Stepdown Rule** — each function leads to the next level of detail.

#### Flag arguments → two functions

```csharp
// ❌
report.Render(isSuite: true);

// ✅
report.RenderForSuite();
report.RenderForSingleTest();
```

#### Too many arguments → parameter object

```csharp
// ❌
CreateUser(string first, string last, string email, string phone, DateOnly dob, string country);

// ✅
public record CreateUserRequest(string FirstName, string LastName, string Email,
                                string Phone, DateOnly DateOfBirth, string Country);
CreateUser(CreateUserRequest request);
```

#### Command–Query Separation

```csharp
// ❌ Does it set the value, or check if it was set? Both!
if (Set("username", "bob")) { ... }

// ✅
if (!AttributeExists("username"))
    SetAttribute("username", "bob");
```

> **.NET connection:** CQS at the method level grows into **CQRS** at the architecture level (MediatR commands vs queries, separate read models).

---

### 2.3 Comments

**Rule:** *Comments are, at best, a necessary evil.* Every comment is a small failure to express yourself in code. Comments rot — the code changes, the comment doesn't, and now it lies.

**Real-world analogy:** If a door needs a sign saying "PUSH," the door is badly designed. Put a flat plate on it instead of a handle.

#### ❌ Bad – comment explains bad code

```csharp
// Check to see if the employee is eligible for full benefits
if ((employee.Flags & HourlyFlag) != 0 && employee.Age > 65) { ... }
```

#### ✅ Good – code explains itself

```csharp
if (employee.IsEligibleForFullBenefits()) { ... }
```

#### Good comments (keep these)

| Type | Example |
|---|---|
| Legal | `// Copyright (c) 2026 Contoso. MIT License.` |
| Explanation of **intent / why** | `// We sort descending because the payment provider rejects out-of-order refunds.` |
| Warning of consequences | `// Not thread-safe: HttpClient handler is shared intentionally.` |
| TODO (tracked, short-lived) | `// TODO(#1234): remove after v2 API migration` |
| Public API docs | `/// <summary>` XML docs on NuGet libraries / public APIs |
| Clarifying a regex or external format | `// Format: yyyy-MM-dd'T'HH:mm:ss.fffZ (ISO 8601 UTC)` |

#### Bad comments (delete these)

- **Redundant:** `i++; // increment i`
- **Journal / changelog comments** — that's what Git is for.
- **Commented-out code** — delete it; Git remembers.
- **Mandated noise:** `/// <summary>Gets the name.</summary> public string Name { get; }`
- **Closing-brace comments:** `} // end foreach` — your function is too long.
- **Position markers:** `// ============ PRIVATE METHODS ============` (use `#region` sparingly, if at all).

---

### 2.4 Formatting

**Rule:** Formatting is communication. Consistency matters more than which style you pick.

- **Newspaper metaphor:** The class name is the headline, public high-level methods at the top, details lower down.
- **Vertical openness:** blank line between concepts.
- **Vertical density:** related lines stay together.
- **Caller above callee** where possible.
- **Team rules beat personal preferences.**

#### In .NET, automate it

```ini
# .editorconfig at the solution root
root = true

[*.cs]
indent_style = space
indent_size = 4
csharp_style_namespace_declarations = file_scoped:warning
csharp_prefer_braces = true:warning
dotnet_style_qualification_for_field = false:warning
dotnet_sort_system_directives_first = true
```

```bash
dotnet format            # fix formatting
dotnet format --verify-no-changes   # fail CI if code isn't formatted
```

Also enable analyzers in `Directory.Build.props`:

```xml
<Project>
  <PropertyGroup>
    <Nullable>enable</Nullable>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <AnalysisLevel>latest-recommended</AnalysisLevel>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  </PropertyGroup>
</Project>
```

For React: **Prettier + ESLint**, same idea.

---

### 2.5 Objects vs Data Structures

**Rule:**
- **Objects** hide data and expose *behavior*.
- **Data structures** expose data and have *no meaningful behavior*.
- Don't create hybrids (public setters for everything **plus** business methods) — you get the worst of both.

**Trade-off:**
- With objects, it's easy to add new *types* (new class), hard to add new *behaviors* (change every class).
- With data structures, it's easy to add new *behaviors* (new function), hard to add new *types*.

#### Anemic hybrid ❌

```csharp
public class BankAccount
{
    public decimal Balance { get; set; }   // anyone can set anything
}

// Business rule leaks everywhere:
if (account.Balance >= amount) account.Balance -= amount;
```

#### Real object ✅

```csharp
public sealed class BankAccount
{
    public decimal Balance { get; private set; }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (amount > Balance) throw new InsufficientFundsException(Balance, amount);
        Balance -= amount;
    }
}
```

#### Pure data structure ✅ (DTOs are fine as data!)

```csharp
public record AccountDto(Guid Id, string Owner, decimal Balance); // for API responses
```

#### Law of Demeter ("talk to friends, not strangers")

A method should only call methods on: itself, its parameters, objects it creates, and its direct fields.

```csharp
// ❌ Train wreck – you know the whole internal structure
var city = order.GetCustomer().GetAddress().GetCity();

// ✅ Ask for what you need
var city = order.ShippingCity;
```

**Real-world analogy:** When you pay at a shop, you don't hand the cashier your wallet and let them dig out cash. You hand them the money.

> **Note:** Fluent APIs like LINQ (`orders.Where(...).Select(...)`) are *not* Demeter violations – each call returns a new value of the same abstraction, not a peek into internals.

---

### 2.6 Error Handling

**Rules:**
1. Use exceptions, not return codes.
2. Write your `try-catch-finally` first when code can fail.
3. Provide context with exceptions (what failed, why).
4. Define exception classes by *how the caller handles them*.
5. **Don't return null.**
6. **Don't pass null.**
7. Use the **Special Case pattern** (Null Object) to avoid special-case code.

#### ❌ Error codes and nulls

```csharp
public Customer? Find(int id) => _db.Customers.Find(id);   // may return null

var customer = repo.Find(5);
if (customer != null)
{
    var orders = customer.Orders;
    if (orders != null) { ... }   // null checks everywhere
}
```

#### ✅ Exceptions with context

```csharp
public sealed class CustomerNotFoundException(int customerId)
    : Exception($"Customer {customerId} was not found.")
{
    public int CustomerId { get; } = customerId;
}

public async Task<Customer> GetAsync(int id, CancellationToken ct) =>
    await _db.Customers.FindAsync([id], ct) ?? throw new CustomerNotFoundException(id);
```

#### ✅ Return empty collections, not null

```csharp
public IReadOnlyList<Order> GetOrders(Guid customerId) =>
    _orders.TryGetValue(customerId, out var list) ? list : [];
```

#### ✅ Special Case / Null Object

```csharp
public interface IDiscountPolicy { decimal Apply(decimal price); }

public sealed class NoDiscount : IDiscountPolicy
{
    public static readonly NoDiscount Instance = new();
    public decimal Apply(decimal price) => price;
}

// Now no "if (discount != null)" anywhere:
var policy = customer.DiscountPolicy ?? NoDiscount.Instance;
var final = policy.Apply(price);
```

#### ✅ Global handling in ASP.NET Core (one place, not 50 try/catches)

```csharp
// Program.cs (.NET 8+)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
app.UseExceptionHandler();

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext http, Exception ex, CancellationToken ct)
    {
        var (status, title) = ex switch
        {
            CustomerNotFoundException => (StatusCodes.Status404NotFound, "Not found"),
            ValidationException       => (StatusCodes.Status400BadRequest, "Validation failed"),
            _                         => (StatusCodes.Status500InternalServerError, "Server error")
        };

        logger.LogError(ex, "Request failed: {Title}", title);
        http.Response.StatusCode = status;
        await http.Response.WriteAsJsonAsync(new ProblemDetails { Status = status, Title = title, Detail = ex.Message }, ct);
        return true;
    }
}
```

> **Modern note:** Many .NET teams use a **Result pattern** (`Result<T>`) for *expected* business failures (e.g., "insufficient funds") and exceptions for *unexpected* ones. That's compatible with the book's spirit: no magic codes, no nulls, explicit intent.

---

### 2.7 Boundaries (Third-Party Code)

**Rule:** Don't let third-party APIs leak all over your code. **Wrap them** behind your own interface, and write **learning tests** to understand them.

**Real-world analogy:** A travel power adapter. Your laptop doesn't care what socket each country uses — the adapter handles it.

```csharp
// Your interface – speaks YOUR language
public interface IPaymentGateway
{
    Task<PaymentResult> ChargeAsync(Money amount, PaymentToken token, CancellationToken ct);
}

// Adapter – the only place that knows Stripe exists
public sealed class StripePaymentGateway(Stripe.PaymentIntentService stripe) : IPaymentGateway
{
    public async Task<PaymentResult> ChargeAsync(Money amount, PaymentToken token, CancellationToken ct)
    {
        var intent = await stripe.CreateAsync(new Stripe.PaymentIntentCreateOptions
        {
            Amount = (long)(amount.Value * 100),
            Currency = amount.Currency.ToLowerInvariant(),
            PaymentMethod = token.Value,
            Confirm = true
        }, cancellationToken: ct);

        return intent.Status == "succeeded"
            ? PaymentResult.Success(intent.Id)
            : PaymentResult.Failed(intent.Status);
    }
}
```

Switching to Adyen or a fake for tests now touches **one class**.

---

### 2.8 Unit Tests

**The Three Laws of TDD:**
1. You may not write production code until you have written a failing unit test.
2. You may not write more of a test than is sufficient to fail (not compiling = failing).
3. You may not write more production code than is sufficient to pass the current failing test.

**Rules:**
- **Test code is as important as production code.** Dirty tests become a liability and get deleted.
- **One concept per test.**
- **F.I.R.S.T.:** **F**ast, **I**ndependent, **R**epeatable, **S**elf-validating, **T**imely.
- **Arrange–Act–Assert** (Build–Operate–Check).

```csharp
// xUnit + FluentAssertions-style (or Shouldly / plain Assert)
public class BankAccountTests
{
    [Fact]
    public void Withdraw_WithSufficientFunds_ReducesBalance()
    {
        // Arrange
        var account = BankAccountBuilder.WithBalance(100m);

        // Act
        account.Withdraw(30m);

        // Assert
        Assert.Equal(70m, account.Balance);
    }

    [Fact]
    public void Withdraw_MoreThanBalance_Throws()
    {
        var account = BankAccountBuilder.WithBalance(10m);

        var act = () => account.Withdraw(50m);

        Assert.Throws<InsufficientFundsException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Withdraw_NonPositiveAmount_Throws(decimal amount)
    {
        var account = BankAccountBuilder.WithBalance(100m);
        Assert.Throws<ArgumentOutOfRangeException>(() => account.Withdraw(amount));
    }
}
```

**Domain-specific testing language:** builders like `BankAccountBuilder.WithBalance(100m)` make tests read like specifications.

---

### 2.9 Classes

**Rules:**
1. **Classes should be small** — measured in *responsibilities*, not lines.
2. **Single Responsibility Principle (SRP):** a class should have one, and only one, reason to change.
3. **High cohesion:** most methods use most fields. Low cohesion = split the class.
4. **Organize for change:** open for extension, closed for modification (OCP).
5. **Depend on abstractions** to isolate from change (DIP).

**Test for SRP:** Can you describe the class in ~25 words without using "and," "or," "but," "if"?

#### ❌ God class

```csharp
public class UserManager
{
    public void Register(User u) { /* validate, hash password, save to DB */ }
    public void SendWelcomeEmail(User u) { /* SMTP */ }
    public string ExportToCsv(IEnumerable<User> users) { /* formatting */ }
    public void LogActivity(string msg) { /* file IO */ }
    public bool IsPasswordStrong(string p) { /* regex */ }
}
```

#### ✅ Cohesive classes

```csharp
public sealed class UserRegistrationService(
    IUserRepository users, IPasswordHasher hasher, IPasswordPolicy policy, IWelcomeEmailSender email) { ... }

public sealed class UserCsvExporter { ... }
public sealed class PasswordPolicy : IPasswordPolicy { ... }
```

---

### 2.10 Systems

**Rule:** **Separate constructing a system from using it.** Startup (wiring objects together) is a separate concern from runtime logic.

**Real-world analogy:** Building a hotel vs. running a hotel. The construction crew leaves before guests arrive.

In .NET this is exactly what **Dependency Injection** in `Program.cs` does:

```csharp
// Program.cs – the ONLY place that knows concrete types ("Main" component)
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShopDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Shop")));
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<IPricingService, PricingService>();
builder.Services.AddScoped<IOrderNotifier, ServiceBusOrderNotifier>();
builder.Services.AddScoped<PlaceOrderHandler>();

var app = builder.Build();
app.MapPost("/orders", async (Order order, PlaceOrderHandler handler, CancellationToken ct) =>
    Results.Created($"/orders/{(await handler.Handle(order, ct))}", null));
app.Run();
```

Other ideas in this chapter:
- **Cross-cutting concerns** (logging, transactions, caching) belong in *aspects* → in .NET: middleware, filters, MediatR pipeline behaviors, decorators (Scrutor).
- **Postpone decisions** until the last responsible moment.
- **Use standards wisely** — only where they add demonstrable value.
- **Domain-Specific Languages** make code read like the business.

---

### 2.11 Emergent Design – 4 Rules of Simple Design

(Originally from Kent Beck.) In priority order, a design is "simple" if it:

1. **Runs all the tests.** A system that can't be verified shouldn't be deployed.
2. **Contains no duplication.**
3. **Expresses the intent of the programmer.**
4. **Minimizes the number of classes and methods.** (Don't over-engineer! Lowest priority.)

#### Removing duplication — Template Method example

```csharp
// ❌ Duplicated
public decimal CalculateFullTimeVacation(Employee e)  { /* calc base */ /* full-time rules */ /* alter for legal min */ }
public decimal CalculatePartTimeVacation(Employee e)  { /* calc base */ /* part-time rules */ /* alter for legal min */ }

// ✅
public abstract class VacationPolicy
{
    public decimal Accrue(Employee e)
    {
        var hours = CalculateBaseHours(e);
        hours = ApplyEmploymentRules(e, hours);   // varies
        return EnforceLegalMinimum(hours);
    }

    private static decimal CalculateBaseHours(Employee e) => e.YearsOfService * 8m;
    protected abstract decimal ApplyEmploymentRules(Employee e, decimal hours);
    private static decimal EnforceLegalMinimum(decimal hours) => Math.Max(hours, 80m);
}

public sealed class FullTimeVacationPolicy : VacationPolicy
{
    protected override decimal ApplyEmploymentRules(Employee e, decimal hours) => hours * 1.5m;
}
```

---

### 2.12 Concurrency

**Rules:**
- **Concurrency is a decoupling strategy** — it separates *what* gets done from *when*.
- It adds overhead and complexity; it does not always improve performance.
- **SRP for concurrency:** keep concurrency code separate from business code.
- **Limit the scope of shared data.** Prefer copies / immutable data.
- **Threads should be as independent as possible.**
- Know your library: thread-safe collections, etc.
- Keep `lock` sections small.
- Test with more threads than processors; run tests often on different platforms.

#### .NET translation

```csharp
// ❌ Shared mutable state
private List<string> _log = new();
Parallel.ForEach(items, i => _log.Add(Process(i)));   // race condition!

// ✅ Immutable results, no shared state
var results = await Task.WhenAll(items.Select(ProcessAsync));

// ✅ When you must share: use thread-safe types
private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();

// ✅ Producer/consumer – decouple "what" from "when"
var channel = Channel.CreateBounded<OrderPlaced>(capacity: 1000);
```

Also in modern .NET:
- `async/await` everywhere for I/O; **never** `.Result` or `.Wait()` (deadlocks).
- Pass `CancellationToken` through all async calls.
- Use `records` and `ImmutableArray<T>` for data shared across threads.
- Use `SemaphoreSlim` instead of `lock` in async code.

---

### 2.13 Successive Refinement

The case-study chapters prove one thing:

> **To write clean code, you must first write dirty code and then clean it.**

Uncle Bob writes an argument parser that works but is ugly, then refactors it in **tiny steps**, running tests after every change. Key lessons:

- It's not enough for code to *work*. Working-but-messy code is a debt.
- **Never make a big change that breaks the tests for long.** Small steps; green bar constantly.
- **Incrementalism:** extract → run tests → rename → run tests → move → run tests.

Your workflow in Visual Studio / Rider:
1. Make it work (tests pass).
2. `Ctrl+R, M` (Extract Method), `Ctrl+R, R` (Rename) — use the IDE's refactorings, they're safe.
3. Run tests (`dotnet test` or Live Unit Testing).
4. Commit. Repeat.

---

### 2.14 Code Smells & Heuristics

A condensed checklist of the most useful smells from Chapter 17:

| Category | Smell | What to do |
|---|---|---|
| **Comments** | Obsolete, redundant, commented-out code | Delete |
| **Environment** | Build requires more than one step; tests require more than one step | `dotnet build` / `dotnet test` must just work |
| **Functions** | Too many args, output args, flag args, dead functions | Parameter objects, split, delete |
| **General** | Duplication (G5) | Extract method/class, polymorphism |
| | Code at wrong level of abstraction | Move up/down to correct class |
| | Base class depending on derivatives | Invert |
| | Too much information (fat interfaces) | Narrow interfaces (ISP) |
| | Dead code | Delete |
| | Feature Envy (method uses another class's data more than its own) | Move the method |
| | Obscured intent / magic numbers | Named constants, explaining variables |
| | Prefer polymorphism to `if/else` or `switch` on type | Strategy / subclasses |
| | Follow standard conventions | `.editorconfig` |
| | Encapsulate conditionals | `if (ShouldBeDeleted(timer))` not `if (timer.HasExpired() && !timer.IsRecurrent())` |
| | Avoid negative conditionals | `if (buffer.ShouldCompact())` not `if (!buffer.ShouldNotCompact())` |
| | Functions should do one thing | Extract |
| | Hidden temporal coupling | Make order explicit via return values |
| **Names** | Non-descriptive, wrong abstraction level, ambiguous | Rename |
| **Tests** | Insufficient tests, skipped trivial tests, ignored tests, slow tests | Test everything that could break; keep them fast |

#### Polymorphism instead of switch — example

```csharp
// ❌ Every new employee type = edit every switch in the codebase
public decimal CalculatePay(Employee e) => e.Type switch
{
    EmployeeType.Salaried   => e.MonthlySalary,
    EmployeeType.Hourly     => e.HoursWorked * e.HourlyRate,
    EmployeeType.Commission => e.BaseSalary + e.Sales * e.CommissionRate,
    _ => throw new InvalidOperationException()
};

// ✅ One switch, hidden in a factory; behavior lives with the type
public abstract class Employee { public abstract decimal CalculatePay(); }
public sealed class SalariedEmployee(decimal monthly) : Employee { public override decimal CalculatePay() => monthly; }
public sealed class HourlyEmployee(decimal hours, decimal rate) : Employee { public override decimal CalculatePay() => hours * rate; }
```

> Uncle Bob's rule: a `switch` on type is tolerable **once**, inside an Abstract Factory that creates the polymorphic objects.

---

## 3. Applying It as a .NET + React + Azure Developer

| Area | Clean Code practice |
|---|---|
| **ASP.NET Core APIs** | Thin controllers/endpoints → call one handler. No business logic in controllers. |
| **EF Core** | Don't leak `DbContext` everywhere; repositories or query handlers per use case. Return DTOs, not entities, from APIs. |
| **Configuration** | Strongly-typed `IOptions<T>` instead of `config["Some:Magic:Key"]` scattered around. |
| **Logging** | Structured logging (`logger.LogInformation("Order {OrderId} placed", id)`), never string concatenation. |
| **React** | Small components that do one thing; custom hooks to extract logic (`useOrders()`); meaningful prop names; no 800-line components. |
| **TypeScript** | Same naming rules; avoid `any` (it's the TS version of "returning null"). |
| **Azure Functions** | One function = one trigger = one responsibility; business logic in a separate class library so it's testable without the Functions host. |
| **Infrastructure as Code (Bicep/Terraform)** | Clean Code applies here too: modules, meaningful names, no copy-paste. |
| **CI (GitHub Actions / Azure DevOps)** | `dotnet format --verify-no-changes`, analyzers as errors, tests required for merge. |

### React example — same principles

```tsx
// ❌ One component fetches, transforms, and renders
// ✅ Hook (logic) + component (presentation)
function useOverdueInvoices(customerId: string) {
  return useQuery({
    queryKey: ["invoices", customerId, "overdue"],
    queryFn: () => invoiceApi.getOverdue(customerId),
  });
}

export function OverdueInvoiceList({ customerId }: { customerId: string }) {
  const { data: invoices = [], isLoading } = useOverdueInvoices(customerId);
  if (isLoading) return <Spinner />;
  return <InvoiceTable invoices={invoices} />;
}
```

---

## 4. Pull-Request Checklist

Copy this into your PR template:

- [ ] Names reveal intent; no abbreviations or magic numbers.
- [ ] Each method does one thing; ≤ ~20 lines; ≤ 3 parameters.
- [ ] No `bool` flag parameters.
- [ ] No duplicated logic.
- [ ] No commented-out code; comments explain *why*, not *what*.
- [ ] No `null` returned for collections; nullable reference types respected.
- [ ] Exceptions carry context; no swallowed exceptions (`catch { }`).
- [ ] Third-party SDKs are wrapped behind our interfaces.
- [ ] New behavior is covered by fast, independent unit tests.
- [ ] Classes have one reason to change.
- [ ] `dotnet format` and analyzers pass.
- [ ] I left the code cleaner than I found it (Boy Scout Rule).

---

## 5. Interview Questions & Answers

**Q1. What is clean code?**
Code that is easy to read, easy to change, and has minimal surprises. It's focused (each part does one thing), has tests, has no duplication, and expresses intent clearly. "Clean code reads like well-written prose."

**Q2. What is the Boy Scout Rule?**
Leave the code cleaner than you found it. Small continuous improvements prevent code rot without needing big rewrites.

**Q3. How small should a function be?**
Small enough to do one thing at one level of abstraction. A practical guideline is under ~20 lines and that you can't extract another meaningfully-named function from it.

**Q4. Why avoid boolean parameters?**
A flag means the function does two things. Split it into two well-named functions (`RenderForSuite()` / `RenderForSingleTest()`).

**Q5. When are comments good?**
When they explain *why* (intent, business reasons, warnings), legal notices, and public API docs. Bad when they explain *what* the code does — fix the code instead.

**Q6. Why not return null?**
It forces every caller to null-check; one missed check = `NullReferenceException`. Return empty collections, throw a meaningful exception, use the Null Object pattern, or use a `Result`/`Option` type. In C#, enable nullable reference types.

**Q7. What's the Law of Demeter?**
A method should only talk to its immediate collaborators, not reach through them (`a.GetB().GetC().DoX()`). It reduces coupling to internal structure.

**Q8. What's F.I.R.S.T.?**
Fast, Independent, Repeatable, Self-validating, Timely — properties of good unit tests.

**Q9. Explain Command-Query Separation.**
A method either changes state (command, returns `void`/`Task`) or returns data (query, no side effects), never both. It makes behavior predictable. CQRS is the architectural extension.

**Q10. What are the 4 Rules of Simple Design?**
Passes all tests → no duplication → expresses intent → minimal classes/methods (in priority order).

**Q11. Objects vs data structures?**
Objects hide data and expose behavior; data structures expose data and have no behavior. Avoid hybrids. DTOs are legitimately data structures; domain entities should be objects.

**Q12. How do you deal with a legacy codebase that's a mess?**
Don't rewrite. Add characterization tests around the part you need to change, then refactor in small steps (Boy Scout Rule), strangling the worst areas gradually.

---

## 6. Modern Nuance – Where Not to Over-Apply

Being a *senior* means knowing the rules **and** when to bend them:

- **"Tiny functions" can be overdone.** Ten 3-line methods that you have to jump between can be harder to read than one clear 15-line method. Optimize for *readability*, not line count.
- **Abstractions have a cost.** Don't create `IFoo` for every class if there is only ever one implementation and no test seam needed. (Though interfaces at architectural boundaries are almost always worth it.)
- **Performance-critical code** (hot paths, `Span<T>`, SIMD) may trade some readability for speed — isolate and comment it.
- **The book's examples are Java (2008).** Modern C# has records, pattern matching, primary constructors, nullable reference types, LINQ — use them; they often make code cleaner than the book's examples.
- **Exceptions vs Result types** is a team decision — be consistent.
- **Team conventions beat individual taste.**

---

## 7. Key Takeaways

1. **Code is for humans.** Compilers don't care about names; your teammates do.
2. **Names matter more than you think.** Spend time on them.
3. **Small, focused functions and classes.** One thing, one reason to change.
4. **Comments are a last resort.** Make the code speak.
5. **No nulls, no error codes, no flag arguments.**
6. **Wrap third-party code.**
7. **Tests are first-class code.** No tests = no safe refactoring = rot.
8. **Clean code is the result of refactoring**, not of getting it right the first time.
9. **Boy Scout Rule** every day.
10. **The only way to go fast is to go well.**

---

*Next read: [02 – The Clean Coder](02-The-Clean-Coder.md) (how a professional behaves) and [03 – Clean Architecture](03-Clean-Architecture.md) (clean code at system scale).*
