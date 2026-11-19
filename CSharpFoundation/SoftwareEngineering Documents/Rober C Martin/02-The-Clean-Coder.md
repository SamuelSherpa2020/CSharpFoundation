# The Clean Coder: A Code of Conduct for Professional Programmers

> **Author:** Robert C. Martin ("Uncle Bob") · **Published:** 2011
> **One-line summary:** *Clean Code* is about how to write good code; *The Clean Coder* is about how to **be** a good professional — how you behave, commit, estimate, handle pressure, and work with people.
> **Examples in this guide:** C# 12 / .NET 8+, xUnit, Reqnroll (SpecFlow successor), Azure DevOps / GitHub Actions.

---

## Table of Contents

1. [What the Book Actually Covers](#1-what-the-book-actually-covers)
2. [Core Teachings with Examples](#2-core-teachings-with-examples)
   - 2.1 [Professionalism](#21-professionalism)
   - 2.2 [Saying No](#22-saying-no)
   - 2.3 [Saying Yes – The Language of Commitment](#23-saying-yes--the-language-of-commitment)
   - 2.4 [Coding](#24-coding)
   - 2.5 [Test-Driven Development](#25-test-driven-development)
   - 2.6 [Practicing](#26-practicing)
   - 2.7 [Acceptance Testing](#27-acceptance-testing)
   - 2.8 [Testing Strategies](#28-testing-strategies)
   - 2.9 [Time Management](#29-time-management)
   - 2.10 [Estimation](#210-estimation)
   - 2.11 [Pressure](#211-pressure)
   - 2.12 [Collaboration](#212-collaboration)
   - 2.13 [Teams and Projects](#213-teams-and-projects)
   - 2.14 [Mentoring, Apprenticeship & Craftsmanship](#214-mentoring-apprenticeship--craftsmanship)
   - 2.15 [Tooling](#215-tooling)
3. [Why This Book Matters for a Future Senior Dev / Project Manager](#3-why-this-book-matters-for-a-future-senior-dev--project-manager)
4. [Daily / Weekly Professional Checklist](#4-daily--weekly-professional-checklist)
5. [Interview Questions & Answers](#5-interview-questions--answers)
6. [Key Takeaways](#6-key-takeaways)

---

## 1. What the Book Actually Covers

This is a **behavior and ethics** book with very little code. It is mostly Uncle Bob's stories (including his own early-career failures) and the lessons he drew from them.

| Chapter | Topic | Core question |
|---|---|---|
| 1 | Professionalism | What does it mean to take responsibility for your work? |
| 2 | Saying No | How do you push back on impossible demands without being difficult? |
| 3 | Saying Yes | How do you make commitments you can actually keep? |
| 4 | Coding | How do you stay focused, avoid burnout, and handle being late? |
| 5 | Test-Driven Development | Why is TDD a professional discipline, not an optional extra? |
| 6 | Practicing | Why do professionals practice outside of "real work"? |
| 7 | Acceptance Testing | How do you know what "done" means before you start? |
| 8 | Testing Strategies | What should QA find? (Answer: nothing.) |
| 9 | Time Management | Meetings, focus, energy, and avoiding "priority inversion." |
| 10 | Estimation | How to give honest estimates (PERT, wideband delphi). |
| 11 | Pressure | How to stay calm and keep your disciplines when things get tough. |
| 12 | Collaboration | Working with other programmers and the business. |
| 13 | Teams and Projects | Why stable, gelled teams beat assembling teams per project. |
| 14 | Mentoring, Apprenticeship, Craftsmanship | How the next generation learns the craft. |
| App. A | Tooling | Source control, IDE, CI, issue tracking, testing tools. |

**Central message:** A professional is someone who **takes responsibility** — for their code, their commitments, their schedule, their skills, and their career. Not their employer. Not their manager. *Them.*

---

## 2. Core Teachings with Examples

### 2.1 Professionalism

**Key ideas:**

1. **Do no harm — to function.** Don't ship bugs knowingly. QA should find *nothing*. Every bug that QA finds is something you should be embarrassed by and learn from.
2. **Do no harm — to structure.** Don't make the code harder to change "just this once" to hit a deadline. Messy code makes you slower *tomorrow*.
3. **Know that it works.** How? **Test it.** All of it. Automated.
4. **Work ethic:** Your career is **your** responsibility, not your employer's. Uncle Bob suggests spending ~**20 hours per week** of your own time on learning and growth on top of the 40 hours you give your employer. (Even if you don't hit 20, the principle holds: invest in yourself continuously.)
5. **Know your field:** design patterns, SOLID, methods (XP, Scrum), disciplines (TDD, CI, pairing), artifacts (UML, flowcharts, state diagrams).
6. **Continuous learning, practice, collaboration, mentoring.**
7. **Know your domain:** If you're building insurance software, learn insurance.
8. **Identify with your employer/customer:** Their problems are your problems.
9. **Humility:** You will make mistakes. Own them. Apologize. Fix them.

**Real-world example:**
A surgeon doesn't say "the hospital didn't train me on the new technique." They learn it. A pilot doesn't skip the checklist because they're late. Software professionals should behave the same way.

#### What "Do no harm to function" looks like in .NET

```csharp
// A professional doesn't push code they "think" works.
// They prove it works.

public class InvoiceCalculatorTests
{
    [Theory]
    [InlineData(100, 0.20, 120)]   // standard VAT
    [InlineData(100, 0.00, 100)]   // zero-rated
    [InlineData(0,   0.20, 0)]     // edge case: empty invoice
    public void Total_IncludesTax(decimal net, decimal taxRate, decimal expected)
    {
        var calculator = new InvoiceCalculator();
        Assert.Equal(expected, calculator.Total(net, taxRate));
    }
}
```

And it's enforced, not optional:

```yaml
# azure-pipelines.yml — a PR cannot merge if tests fail or coverage drops
- script: dotnet test --configuration Release --collect:"XPlat Code Coverage"
  displayName: 'Run tests'
```

---

### 2.2 Saying No

**Key ideas:**

- Professionals **speak truth to power.** If a deadline is impossible, saying "we'll try" is lying.
- **Adversarial roles are healthy.** The manager's job is to push for the deadline; your job is to defend what's realistic. The best outcome comes from *both* sides arguing hard and honestly.
- **"Trying" is a commitment** to work harder/longer — if you had extra effort available, you'd already be using it. So "I'll try" usually means "I'll pretend."
- **The cost of saying yes** to an impossible deadline: you rush, skip tests, make a mess, ship late anyway, with bugs.
- **Being a team player** ≠ saying yes to everything. It means being honest so the team can plan.

**Real-world scenario (the classic from the book, adapted):**

> **PM:** "We need the new checkout flow live by Friday for the marketing campaign."
> **Junior dev:** "Okay, I'll try." *(works until 2 a.m., skips tests, ships Friday with a payment bug)*
> **Professional dev:** "Friday isn't possible for the whole flow. Here's what I *can* do by Friday: guest checkout with card payments, behind a feature flag. Saved addresses and PayPal will take until the following Wednesday. Which matters most for the campaign?"

The professional said **no** to the impossible, but offered **options**. That is the goal: find a *yes* that is true.

#### How to say no without being "difficult"

| Instead of… | Say… |
|---|---|
| "That's impossible." | "That's not possible by Friday. Here's what is." |
| "I'll try." | "I can commit to X. I can't commit to Y." |
| "Fine, whatever." | "I want to hit this date too. Let's look at scope." |
| (silence, then miss) | "I'm raising this now so we're not surprised later." |

#### Code tie-in: feature flags make "partial yes" possible

```csharp
// Ship a smaller, finished slice safely. Microsoft.FeatureManagement + Azure App Configuration
builder.Services.AddFeatureManagement();

app.MapPost("/checkout", async (CheckoutRequest req, IFeatureManager features, CheckoutService svc) =>
{
    if (req.PaymentMethod == PaymentMethod.PayPal && !await features.IsEnabledAsync("PayPalCheckout"))
        return Results.BadRequest("PayPal is not available yet.");

    return Results.Ok(await svc.CheckoutAsync(req));
});
```

---

### 2.3 Saying Yes – The Language of Commitment

A real commitment has three parts: **Say. Mean. Do.**

1. **You say you'll do it.**
2. **You mean it.**
3. **You actually do it.**

**Words that signal lack of commitment** (watch for them in yourself and your team):
- "**Need/should**" → "We *need* to get this done." "I *should* lose weight." (A wish, not a commitment.)
- "**Hope/wish**" → "I *hope* we can finish by Thursday."
- "**Let's**" → "*Let's* meet sometime." (Who? When?)

**Words of commitment:** "**I will** … **by** …"

> "I will have the payment API merged to main, with tests, by Thursday 3 p.m."

**What if it depends on someone else?** You can only commit to things *you* control. So commit to the actions you control:

> "I can't guarantee the Azure subscription will be approved by Monday. **I will** submit the request today, follow up Wednesday, and build against the local emulator (Azurite) meanwhile so we're not blocked."

**What if you can't keep it?** Raise a red flag **as early as possible**. A commitment you're going to miss, flagged early, is still professional behavior. Flagged late, it's a disaster.

#### Turning commitments into checkable artifacts

A "yes" is only real if "done" is defined:

```markdown
## Definition of Done (team-wide)
- [ ] Code merged to main via PR with ≥1 approval
- [ ] Unit tests written and passing
- [ ] Acceptance tests for the story passing
- [ ] No new analyzer warnings
- [ ] Deployed to the Staging slot in Azure App Service
- [ ] PO has seen it working
```

---

### 2.4 Coding

**Key ideas:**

1. **Readiness.** Coding requires concentration. If you're tired, worried, or distracted, you'll write code you'll have to throw away. **Don't code at 3 a.m.** Don't code when angry or anxious.
2. **The "Flow Zone" is overrated.** Being "in the zone" feels productive but you lose the big picture and stop listening. Uncle Bob suggests avoiding it (or at least being aware of it). **Pair programming** keeps you out of the tunnel-vision zone.
3. **Music:** may help you focus, but it may also be occupying the part of your brain you need. Know yourself.
4. **Interruptions:** Be polite. A pair partner or TDD helps you get back to where you were — the failing test tells you exactly what's next.
5. **Writer's block:** Find a pair partner. Get enough sleep. Feed your creativity (read, play, explore).
6. **Debugging time is coding time.** TDD drastically reduces it.
7. **Pacing yourself:** Software development is a **marathon, not a sprint**. Walk away from a hard problem — the solution often comes in the shower or on a walk.
8. **Being late:** You *will* be late sometimes. The key is to **know early** and **communicate**. Don't hope. Use estimates with ranges; update them constantly.
9. **Hope is the project killer.** Don't let hope substitute for data.
10. **Don't rush.** There is no way to code faster under pressure — there's only cutting corners, which makes you slower.
11. **Overtime:** Only acceptable if (a) you can personally afford it, (b) it's short (≤ 2 weeks), and (c) your boss has a fallback plan if it fails.
12. **False delivery:** Saying "done" when it isn't is the worst unprofessional act. Define "done" (acceptance tests!).
13. **Help:** Ask for help and give help. It's unprofessional to stay stuck for hours out of pride.

**Real-world example:**
You're stuck on an EF Core migration bug for 3 hours at 11 p.m. The professional move is not to keep going. It's to stop, sleep, and either solve it fresh in 20 minutes the next morning or ask a teammate. "Grinding" produces hacks.

#### TDD as an "interruption recovery" tool

```csharp
// You were interrupted. When you come back, the red test tells you exactly where you were.
[Fact]
public void ApplyCoupon_ExpiredCoupon_ThrowsCouponExpiredException()   // ❌ RED
{
    var cart = new Cart(clock: new FakeClock(new DateTime(2026, 11, 19)));
    var expired = new Coupon("BLACKFRIDAY", expiresOn: new DateTime(2026, 11, 1));

    Assert.Throws<CouponExpiredException>(() => cart.ApplyCoupon(expired));
}
```

---

### 2.5 Test-Driven Development

**Uncle Bob's position:** *"The jury is in."* TDD is a professional discipline, like double-entry bookkeeping for accountants or sterile procedure for surgeons.

**The Three Laws of TDD:**
1. Don't write production code until you have a failing unit test.
2. Don't write more of a test than is sufficient to fail (compile errors count as failures).
3. Don't write more production code than is sufficient to pass the currently failing test.

The cycle is ~30 seconds to a few minutes: **Red → Green → Refactor**.

**Benefits:**
- **Certainty:** you know your code works because you have tests for everything.
- **Defect injection rate** drops dramatically.
- **Courage:** you can refactor fearlessly — tests catch regressions. Without tests, people stop cleaning code because they're afraid to break it.
- **Documentation:** tests are executable, always-up-to-date examples of how to use the code.
- **Design:** testable code is decoupled code. TDD forces good design.

**What TDD is not:** It's not a religion, it doesn't guarantee good design, and there are cases (UI layout, exploratory spikes) where it's impractical. But "professional default" = TDD.

#### A real TDD session in C# — "Password strength" rule

**Requirement:** A password is valid if it has ≥ 12 characters, at least one digit, at least one uppercase letter.

**Step 1 — RED**

```csharp
public class PasswordPolicyTests
{
    private readonly PasswordPolicy _policy = new();

    [Fact]
    public void ShortPassword_IsInvalid() =>
        Assert.False(_policy.IsValid("Abc1"));
}
```

Doesn't compile → that's "failing." Create the class:

**Step 2 — GREEN (simplest possible)**

```csharp
public sealed class PasswordPolicy
{
    public bool IsValid(string password) => false;
}
```

**Step 3 — RED again (force real behavior)**

```csharp
[Fact]
public void LongPasswordWithDigitAndUppercase_IsValid() =>
    Assert.True(_policy.IsValid("CorrectHorse42"));
```

**Step 4 — GREEN**

```csharp
public bool IsValid(string password) => password.Length >= 12;
```

**Step 5 — RED**

```csharp
[Fact]
public void PasswordWithoutDigit_IsInvalid() =>
    Assert.False(_policy.IsValid("CorrectHorseBattery"));

[Fact]
public void PasswordWithoutUppercase_IsInvalid() =>
    Assert.False(_policy.IsValid("correcthorse42"));
```

**Step 6 — GREEN + REFACTOR**

```csharp
public sealed class PasswordPolicy
{
    private const int MinimumLength = 12;

    public bool IsValid(string password) =>
        IsLongEnough(password) && HasDigit(password) && HasUppercase(password);

    private static bool IsLongEnough(string p) => p.Length >= MinimumLength;
    private static bool HasDigit(string p) => p.Any(char.IsDigit);
    private static bool HasUppercase(string p) => p.Any(char.IsUpper);
}
```

Each step was tiny. Every moment you had a working system. That's the discipline.

---

### 2.6 Practicing

**Key idea:** Musicians practice scales. Martial artists practice kata. Athletes train. **Professional programmers practice** — outside of production work, where failure is free.

**Ways to practice:**
- **Katas:** Solve the same small problem repeatedly until the keystrokes are automatic and you can focus on *technique* (e.g., Bowling Game, Prime Factors, Roman Numerals, Word Wrap, String Calculator, Gilded Rose for refactoring).
- **Wasa:** Pair kata — one writes a test, the other makes it pass, swap.
- **Randori:** Group kata — rotating at one keyboard on a projector.
- **Broadening experience:** Contribute to open source; learn a different language every year (F#, Rust, Go, TypeScript…).

**Weekly plan for you (.NET + React + Azure):**

| Day | 30-minute practice |
|---|---|
| Mon | C# kata with strict TDD (e.g., Roman Numerals) |
| Tue | Refactoring kata (Gilded Rose in C#) |
| Wed | React kata – build a small component test-first with React Testing Library |
| Thu | Azure – deploy something tiny with Bicep from scratch, then tear it down |
| Fri | Read source code of a library you use (ASP.NET Core, MediatR, Polly) |
| Weekend | One small open-source contribution or side project |

#### Kata example: Prime Factors (C#)

```csharp
public static class PrimeFactors
{
    public static IReadOnlyList<int> Of(int n)
    {
        var factors = new List<int>();
        for (var divisor = 2; n > 1; divisor++)
            for (; n % divisor == 0; n /= divisor)
                factors.Add(divisor);
        return factors;
    }
}

public class PrimeFactorsTests
{
    [Theory]
    [InlineData(1,   new int[] { })]
    [InlineData(2,   new[] { 2 })]
    [InlineData(4,   new[] { 2, 2 })]
    [InlineData(12,  new[] { 2, 2, 3 })]
    [InlineData(100, new[] { 2, 2, 5, 5 })]
    public void Factors(int n, int[] expected) => Assert.Equal(expected, PrimeFactors.Of(n));
}
```

Do it test-by-test (1, 2, 3, 4, 6, 8, 9…) and watch the algorithm *emerge*. That's the point of the kata.

---

### 2.7 Acceptance Testing

**Key ideas:**

- **The biggest communication problem** between business and developers is **ambiguity** and **premature precision**.
- **"Done" must be unambiguous.** Acceptance tests define "done."
- Acceptance tests are written **by business analysts / QA with developers**, **before** or at the start of the story.
- They must be **automated**. Manual acceptance tests are too expensive and get skipped.
- Developers' job: **connect** the acceptance tests to the system and make them pass.
- **Unit tests** = programmers' spec of *how* the code works. **Acceptance tests** = business's spec of *what* the system does.
- **GUI tests are brittle.** Test business rules through an API/service layer, not through the UI.

**Real-world scenario:**
PO says, "Users should get a discount if they're loyal." What's "loyal"? What discount? Instead of guessing, you write the acceptance test together:

```gherkin
# Features/LoyaltyDiscount.feature  (Reqnroll / SpecFlow – Gherkin)
Feature: Loyalty discount

  Scenario: Customer with more than 5 orders in the last year gets 10% off
    Given a customer who placed 6 orders in the last 12 months
    When they check out a cart worth 200.00 EUR
    Then the order total should be 180.00 EUR

  Scenario: Customer with exactly 5 orders gets no discount
    Given a customer who placed 5 orders in the last 12 months
    When they check out a cart worth 200.00 EUR
    Then the order total should be 200.00 EUR
```

Now "loyal" means "> 5 orders in 12 months" — **exactly**. The edge case (5) is explicit.

```csharp
// StepDefinitions/LoyaltyDiscountSteps.cs
[Binding]
public sealed class LoyaltyDiscountSteps(CheckoutTestDriver driver)
{
    [Given(@"a customer who placed (\d+) orders in the last 12 months")]
    public void GivenCustomerWithOrders(int count) => driver.CreateCustomerWithRecentOrders(count);

    [When(@"they check out a cart worth (.*) EUR")]
    public async Task WhenTheyCheckOut(decimal amount) => await driver.CheckoutAsync(amount);

    [Then(@"the order total should be (.*) EUR")]
    public void ThenTotalShouldBe(decimal expected) => Assert.Equal(expected, driver.LastOrderTotal);
}
```

`CheckoutTestDriver` talks to the application layer (or `WebApplicationFactory<Program>` for API-level), **not** the React UI.

---

### 2.8 Testing Strategies

**Key ideas:**

- **QA should find nothing.** The dev team's goal is zero bugs reaching QA. When QA finds a bug, the team should be surprised and investigate *why* their process let it through.
- **QA is part of the team**, as specifiers (writing acceptance tests) and characterizers (exploratory testing to discover the system's real behavior).
- **The Test Automation Pyramid:**

```
                 /\
                /  \      Exploratory (manual)        ~ few
               /----\
              / GUI  \    End-to-end / UI tests        ~ 5%
             /--------\
            /Integration\  Component + integration     ~ 10–20%
           /------------\
          /  Acceptance   \ (business rules via API)   ~ 20%
         /----------------\
        /    Unit tests     \                          ~ 100% of business logic
       /--------------------\
```

#### The pyramid in a .NET + React + Azure project

| Layer | Tool | What it tests |
|---|---|---|
| Unit | xUnit / NUnit, Moq / NSubstitute; Vitest/Jest for React | Pure logic, components in isolation |
| Component | `WebApplicationFactory<Program>`, Testcontainers (real SQL Server in Docker) | One service end-to-end with real DB |
| Integration | Testcontainers, Azurite, Service Bus emulator | Service ↔ infrastructure |
| Acceptance | Reqnroll (Gherkin) against API | Business rules, "done" |
| UI / E2E | Playwright (.NET or TS) | A few critical user journeys only |
| Exploratory | Humans | What nobody thought of |

```csharp
// Component test – real HTTP pipeline, real DI, in-memory host
public class OrdersApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task PostOrder_ReturnsCreated()
    {
        var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/orders", new { customerId = Guid.NewGuid(), items = new[] { new { sku = "ABC", qty = 1 } } });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
```

---

### 2.9 Time Management

**Key ideas:**

1. **Meetings are expensive.** A 1-hour meeting with 8 people costs a full working day.
2. **You can decline meetings.** Your most precious resource is time. If a meeting doesn't need you, politely decline or leave.
3. **Every meeting needs an agenda and a goal.** If it doesn't have one, ask for it.
4. **Stand-ups:** ~20 seconds per person, three questions (what I did, what I'll do, what's blocking me).
5. **Iteration planning:** should take ≤ 5% of the iteration.
6. **Retrospectives:** short (~20 min retro + ~25 min demo for a 2-week iteration).
7. **Arguments:** Any argument that can't be settled in 5 minutes can't be settled by arguing. **Get data** (build a spike, run a benchmark, A/B test).
8. **Focus-manna:** Concentration is a limited daily resource. Spend it on hard work; recharge with sleep, exercise, breaks, (moderate) caffeine.
9. **Pomodoro technique:** 25 min focused work, 5 min break. Defend the tomato; batch interruptions.
10. **Avoidance & priority inversion:** Doing an easy, fun task to avoid the important, scary one. Recognize it; stop it.
11. **Blind alleys:** You'll go down wrong paths. The professional skill is recognizing it early and **backing out** — don't be emotionally attached to a failing approach.
12. **Marshes, bogs, swamps & messes:** Bad code slows you down more than a blind alley. When you realize your design is a mess, **turn back immediately**. The longer you push forward, the more expensive it gets.

**Real-world example for a lead/PM:**
You have a recurring 1-hour "status sync" with 10 engineers. That's 10 engineer-hours/week (~1.3 days). Replace it with an async written update in Teams + a 15-minute stand-up, and you just gained ~1 engineer-day back per week.

#### "Get data, don't argue" — in code

Two seniors argue whether `string.Concat` or `StringBuilder` is faster in a hot loop. Don't argue — **measure**:

```csharp
// BenchmarkDotNet – settles arguments with data
[MemoryDiagnoser]
public class StringBuildingBenchmarks
{
    [Params(10, 1000)] public int N;

    [Benchmark(Baseline = true)]
    public string Concat()
    {
        var s = "";
        for (var i = 0; i < N; i++) s += i;
        return s;
    }

    [Benchmark]
    public string Builder()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < N; i++) sb.Append(i);
        return sb.ToString();
    }
}
// dotnet run -c Release
```

---

### 2.10 Estimation

**Key ideas:**

- **Businesses see estimates as commitments. Developers see them as guesses.** That gap causes most conflict.
- **A commitment is something you must achieve.** Only commit when you're *sure*.
- **An estimate is a guess** — and should be given as a **probability distribution**, not a single number.
- **Implied commitments** are dangerous: "I'll try to get it done by Friday" is heard as "Friday."

#### PERT (Program Evaluation and Review Technique)

For each task, give three numbers:
- **O** — Optimistic (everything goes right; ~1% chance)
- **N** — Nominal (most likely)
- **P** — Pessimistic (everything goes wrong, short of disaster; ~1% chance)

Then:

- **Expected duration μ = (O + 4N + P) / 6**
- **Standard deviation σ = (P − O) / 6**
- For a sequence of tasks: **μ_total = Σμ**, **σ_total = √(Σσ²)**

#### PERT in C#

```csharp
public readonly record struct PertEstimate(double Optimistic, double Nominal, double Pessimistic)
{
    public double Expected => (Optimistic + 4 * Nominal + Pessimistic) / 6;
    public double StdDev   => (Pessimistic - Optimistic) / 6;
}

public static class PertCalculator
{
    public static (double Expected, double StdDev) Combine(IEnumerable<PertEstimate> tasks)
    {
        var list = tasks.ToList();
        var expected = list.Sum(t => t.Expected);
        var stdDev = Math.Sqrt(list.Sum(t => t.StdDev * t.StdDev));
        return (expected, stdDev);
    }
}

// Usage – estimating a feature in days
var tasks = new[]
{
    new PertEstimate(1, 2, 6),   // API endpoint + tests
    new PertEstimate(1, 3, 8),   // React screens
    new PertEstimate(0.5, 1, 4), // Azure infra (Bicep)
    new PertEstimate(1, 2, 10),  // Third-party payment integration
};

var (mu, sigma) = PertCalculator.Combine(tasks);
Console.WriteLine($"Expected: {mu:F1} days, σ = {sigma:F1}");
Console.WriteLine($"~84% confident: ≤ {mu + sigma:F1} days");
Console.WriteLine($"~98% confident: ≤ {mu + 2 * sigma:F1} days");
// Expected: 10.6 days, σ ≈ 2.2  →  "Most likely ~10–11 days; very likely under ~15."
```

**This is how you answer "when will it be done?" like a senior:**
> "Most likely around 10–11 working days. I'm ~85% confident it's under 13, and very confident it's under 15. The biggest risk is the payment integration — if we can get sandbox access this week, the range tightens a lot."

#### Group estimation techniques

- **Wideband Delphi:** a group discusses, estimates independently, discusses disagreements, repeats until consensus.
- **Flying Fingers / Planning Poker:** everyone reveals simultaneously (prevents anchoring on the senior's number).
- **Affinity Estimation:** sort cards by relative size on a table, silently.
- **Law of Large Numbers:** break big tasks into many small ones; errors average out.

---

### 2.11 Pressure

**Key ideas:**

1. **Avoid pressure** in the first place:
   - Don't make commitments you can't keep.
   - **Keep the system, code, and design clean.** Messes cause pressure.
   - **Crisis discipline:** "Choose disciplines you feel comfortable following in a crisis. Then follow them *all the time*." If TDD is something you drop under pressure, you don't really believe in it.
2. **Handling pressure:**
   - **Don't panic.** Manage your stress. Sleep. Think about the problem.
   - **Communicate.** Let your team and leaders know you're in trouble. Avoid surprises.
   - **Rely on your disciplines.** When under pressure, follow TDD *more* strictly, not less.
   - **Get help.** Pair with someone.

**Real-world example — Production incident at 2 p.m. Friday:**

| Panicked developer | Professional developer |
|---|---|
| SSH/Kudu into prod, edit config live | Check Application Insights, find root cause |
| Push a hotfix straight to main without tests | Write a failing test that reproduces the bug |
| Deploy directly to production | Fix, PR (fast-track review), deploy to staging slot |
| Break something else | **Swap** staging ↔ production slot (instant rollback available) |
| Nobody knows what happened | Post-incident review (blameless), add monitoring alert |

```bash
# Azure App Service – deploy to staging slot, verify, then swap (zero-downtime, reversible)
az webapp deployment slot swap --resource-group rg-shop --name app-shop-api --slot staging --target-slot production
```

---

### 2.12 Collaboration

**Key ideas:**

- Many programmers became programmers because they prefer machines to people. **That's not an excuse.** Professionals work with people.
- **Employers:** Understand the business goals. Your job is to solve business problems, not just write code.
- **Programmers:** **Don't own code individually.** "My module" is a smell. **Collective ownership** — anyone can improve any code.
- **Pairing:** The most efficient way to solve problems, share knowledge, and review code.
- **Cerebellum:** Some people work best alone at times — fine, but don't make it the norm.

**Practical habits:**
- Use PRs as conversations, not gatekeeping.
- Rotate who works on which area so there's no "bus factor = 1."
- Explain business impact in standups ("this unblocks invoicing for Finance"), not just tickets.

---

### 2.13 Teams and Projects

**Key ideas:**

- **The gelled team:** A team takes time to "gel" (Uncle Bob suggests it can take ~6 months to a year). Once gelled, it's extraordinarily productive.
- **Don't build teams around projects; give projects to teams.** Keep stable teams together and feed them work. Don't disband a gelled team when a project ends.
- **Avoid splitting people across multiple projects** ("50% on Project A, 50% on B"). Context switching kills productivity; people don't really split.
- **Ideal team:** ~12 people (programmers, testers, analysts, a project manager) — modern guidance often says smaller (5–9).
- **Velocity** of a team is the business's lever: business allocates work by prioritizing, not by moving people around.

**For your future PM role:**
- Measure team velocity, not individual output.
- Protect teams from being raided for "urgent" side projects.
- Let the team own the "how"; the business owns the "what" and "when" (priority).

---

### 2.14 Mentoring, Apprenticeship & Craftsmanship

**Key ideas:**

- **Degrees are not enough.** Many graduates can't code professionally. Learning happens through **mentoring**.
- Uncle Bob proposes an **apprenticeship model** like medicine/trades:
  - **Apprentice/Intern** (~1 year+): works under close supervision, no autonomy, pairs constantly.
  - **Journeyman** (~5 years): competent, works on teams, still under masters' oversight; learns many technologies.
  - **Master** (10+ years): has led many projects; mentors others; sets technical direction.
- **Craftsmanship** is a *mindset*: skill + pride + responsibility. It is transmitted **person to person**, not via certificates.

**What this means for you:**
To become a top-1% developer, you need **both**: be mentored by masters (read their code, pair with them, ask for brutal reviews), *and* mentor juniors (teaching is the fastest way to deepen mastery).

---

### 2.15 Tooling

The book's appendix recommends a minimal, solid toolkit. Modern .NET equivalent:

| Need | Book's advice | Your modern stack |
|---|---|---|
| Source control | Use it always; prefer simple, distributed | **Git** (GitHub / Azure Repos), trunk-based with short-lived branches |
| IDE | Know your editor deeply | **Visual Studio / Rider / VS Code** — learn every refactoring shortcut |
| Issue tracking | Keep it simple | **Azure Boards / Jira / GitHub Issues** |
| Continuous build | Never break the build; fix immediately | **GitHub Actions / Azure Pipelines** on every PR |
| Unit testing | Must be fast, one-click | **xUnit**, **Vitest/Jest** |
| Component testing | Test business rules via API | **WebApplicationFactory**, **Testcontainers** |
| Acceptance testing | Business-readable | **Reqnroll** (Gherkin), **FitNesse** (book's tool) |
| UI testing | Few, critical paths | **Playwright** |
| Monitoring | — | **Application Insights / Azure Monitor** |

---

## 3. Why This Book Matters for a Future Senior Dev / Project Manager

| As a… | What you'll use from this book |
|---|---|
| **Senior Developer** | Honest estimates (PERT), saying no with options, TDD under pressure, mentoring juniors |
| **Tech Lead** | Definition of Done, acceptance tests with the PO, the testing pyramid, protecting focus time |
| **Project Manager** | Estimates ≠ commitments; stable teams; velocity over headcount; fewer, better meetings; data over arguments |
| **Interviewee** | Behavioral questions ("Tell me about a time you pushed back on a deadline") — this book gives you the framework |

**The 1% mindset from this book:**
1. You own your career. Invest time weekly.
2. You never knowingly ship broken code.
3. You never say "I'll try" when you mean "probably not."
4. Your estimates are ranges with probabilities.
5. Your disciplines don't disappear under pressure.

---

## 4. Daily / Weekly Professional Checklist

**Daily**
- [ ] I know exactly what I committed to today, and to whom.
- [ ] I wrote tests before/with my code.
- [ ] I flagged any risk to my commitments **today**, not tomorrow.
- [ ] I protected at least 2 blocks of focused time.
- [ ] I asked for help if I was stuck > 1 hour.
- [ ] I stopped coding when I was too tired to do it well.

**Weekly**
- [ ] I practiced (kata, new tech, reading code) outside of tickets.
- [ ] I helped or mentored someone.
- [ ] I declined or shortened at least one low-value meeting.
- [ ] I reviewed my estimates vs actuals to calibrate.

---

## 5. Interview Questions & Answers

**Q1. Tell me about a time you pushed back on a deadline.** *(Behavioral)*
Use the book's framework: *Situation* (unrealistic date) → *I said no to the impossible* → *I offered options* (reduced scope, phased delivery, feature flags) → *Outcome* (delivered the critical slice on time, rest a week later, zero production bugs).

**Q2. What's the difference between an estimate and a commitment?**
An estimate is a probabilistic guess (a range). A commitment is a promise you must keep. Professionals don't let estimates be heard as commitments; they give ranges (e.g., PERT) and only commit to what they control.

**Q3. How do you estimate a task?**
Break it down into small tasks, estimate each with optimistic/nominal/pessimistic values (PERT), combine them, and communicate the expected value with a confidence range. Re-estimate as I learn more. For team estimates, use planning poker / wideband delphi to avoid anchoring.

**Q4. What does "done" mean to you?**
Done means meeting the team's Definition of Done: code merged, tested (unit + acceptance), reviewed, deployed to a staging environment, and accepted by the product owner. "Done except for testing" isn't done.

**Q5. Is TDD worth it? Why?**
Yes as a default discipline: it gives certainty, reduces defects and debugging time, enables fearless refactoring, documents behavior, and pushes toward decoupled design. I adapt it pragmatically for exploratory spikes and UI layout.

**Q6. What should QA find?**
Ideally nothing. QA's main value is specifying acceptance tests up front and exploratory testing. If QA finds bugs, the dev process has a gap we should fix.

**Q7. How do you handle pressure / production incidents?**
Stay calm, communicate early, rely on disciplines (reproduce with a test, fix, review, deploy via safe path with rollback), get help, then do a blameless retrospective.

**Q8. How do you keep your skills up to date?**
Regular deliberate practice (katas, side projects), reading (books, source code), learning a new language/tech each year, contributing to OSS, and mentoring others.

**Q9. What's priority inversion?**
Working on easy/fun tasks to avoid harder, more important ones. I prevent it by prioritizing explicitly each morning and timeboxing.

**Q10. When is overtime acceptable?**
Only short-term (≤ ~2 weeks), when I can personally afford it, and when there's a fallback plan if it fails. Sustained overtime reduces quality and velocity.

---

## 6. Key Takeaways

1. **Professionalism = responsibility.** For code quality, commitments, and your own growth.
2. **Say no** to impossible demands — and always offer an honest alternative.
3. **Commit with "I will … by …"** — and raise flags early.
4. **Don't code tired, angry, or rushed.** It costs more than it saves.
5. **TDD is a professional discipline**, not an optional technique.
6. **Practice deliberately**, like any other craft.
7. **Acceptance tests define "done"** — automated, written with the business.
8. **QA should find nothing.**
9. **Guard your time and focus.** Decline useless meetings; resolve arguments with data.
10. **Estimates are probability distributions** (PERT), not promises.
11. **Under pressure, follow your disciplines more strictly.**
12. **Keep teams stable**, own code collectively, mentor and be mentored.

---

*Previous: [01 – Clean Code](01-Clean-Code.md) · Next: [03 – Clean Architecture](03-Clean-Architecture.md)*
