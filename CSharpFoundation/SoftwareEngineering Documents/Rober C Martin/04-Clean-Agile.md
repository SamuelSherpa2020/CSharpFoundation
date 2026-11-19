# Clean Agile: Back to Basics

> **Author:** Robert C. Martin ("Uncle Bob") · **Published:** 2019
> **One-line summary:** Agile is a **small idea about small teams doing small things**. It produces **data** so managers can make good decisions early, and it relies on **technical discipline** to stay fast. Strip away the certifications and ceremonies, and that's what's left.
> **Examples in this guide:** C# 12 / .NET 8+, xUnit, Reqnroll, GitHub Actions / Azure Pipelines, Azure.

---

## Table of Contents

1. [What the Book Actually Covers](#1-what-the-book-actually-covers)
2. [Core Teachings with Examples](#2-core-teachings-with-examples)
   - 2.1 [What Agile Really Is (and Isn't)](#21-what-agile-really-is-and-isnt)
   - 2.2 [The Iron Cross of Project Management](#22-the-iron-cross-of-project-management)
   - 2.3 [Agile Produces Data – Velocity & Burn-down](#23-agile-produces-data--velocity--burn-down)
   - 2.4 [The Circle of Life – XP Practices](#24-the-circle-of-life--xp-practices)
   - 2.5 [Reasons for Agile – Reasonable Expectations](#25-reasons-for-agile--reasonable-expectations)
   - 2.6 [The Bill of Rights](#26-the-bill-of-rights)
   - 2.7 [Business Practices](#27-business-practices)
   - 2.8 [Team Practices](#28-team-practices)
   - 2.9 [Technical Practices](#29-technical-practices)
   - 2.10 [Becoming Agile](#210-becoming-agile)
   - 2.11 [Craftsmanship](#211-craftsmanship)
3. [A Full Iteration – Walkthrough for a .NET + React + Azure Team](#3-a-full-iteration--walkthrough-for-a-net--react--azure-team)
4. [For Your Future Project Manager Role](#4-for-your-future-project-manager-role)
5. [Planning Checklist](#5-planning-checklist)
6. [Interview Questions & Answers](#6-interview-questions--answers)
7. [Key Takeaways](#7-key-takeaways)

---

## 1. What the Book Actually Covers

Uncle Bob was one of the 17 people at the **Snowbird meeting (February 2001)** that wrote the **Agile Manifesto**. Almost 20 years later, he felt the original message had been diluted — by certification industries, by "Agile" that is just Scrum ceremonies without engineering discipline, and by project managers using Agile as a way to pressure teams.

This book returns to the **original** idea.

| Chapter | Topic |
|---|---|
| 1 | **Introduction to Agile** – history, the Manifesto, the Iron Cross, charts, why Agile is about data |
| 2 | **Reasons for Agile** – professionalism, reasonable customer expectations, the Bill of Rights |
| 3 | **Business Practices** – Planning Game, Small Releases, Acceptance Tests, Whole Team |
| 4 | **Team Practices** – Metaphor, Sustainable Pace, Collective Ownership, Continuous Integration, Stand-ups |
| 5 | **Technical Practices** – TDD, Refactoring, Simple Design, Pair Programming |
| 6 | **Becoming Agile** – values, transformations, coaching, certification, Agile in the large, tools |
| 7 | **Craftsmanship** (guest chapter by Sandro Mancuso) – the Software Craftsmanship movement |
| 8 | **Conclusion** |

### The Agile Manifesto (values — paraphrased)

We value:
- **Individuals and interactions** over processes and tools
- **Working software** over comprehensive documentation
- **Customer collaboration** over contract negotiation
- **Responding to change** over following a plan

*While there is value in the items on the right, we value the items on the left more.*

---

## 2. Core Teachings with Examples

### 2.1 What Agile Really Is (and Isn't)

**Agile IS:**
- A way for **small** software teams to do **small** projects well.
- A **feedback-driven** process: short cycles, measurable progress, early truth.
- A set of **disciplines** — business, team, and technical — that must all be present.

**Agile is NOT:**
- A way to go faster by magic. ("Agile won't make you go faster. It will tell you *how fast you're actually going*, early.")
- Just stand-ups and sprints. **Agile without technical practices (TDD, refactoring, CI) collapses** — Uncle Bob calls this "Flaccid Scrum" (a term coined by Martin Fowler).
- A certification. ("Agile certifications are a joke.")
- A big-team or enterprise framework by origin. (Agile in the large is mostly the same as small teams, multiplied, with management of the interdependencies.)

**Real-world example:**
A company adopts "Agile" by renaming project phases to "sprints," adding a daily 45-minute status meeting, and buying Jira. No automated tests, no CI, no refactoring. After 6 months, velocity drops every sprint, every release breaks something, and management concludes "Agile doesn't work." It never was Agile.

---

### 2.2 The Iron Cross of Project Management

> **Good, Fast, Cheap, Done: pick any three.** You can't have all four.

| Want… | Sacrifice… |
|---|---|
| Good + Fast + Cheap | Not done (scope cut) |
| Good + Fast + Done | Not cheap (more people/money) |
| Good + Cheap + Done | Not fast (takes longer) |
| Fast + Cheap + Done | **Not good** ← *the one Agile says you must never cut* |

**A good manager doesn't demand all four at 100%.** They adjust the coefficients — maybe 80% scope, slightly later date, a bit more budget — based on **data**.

**In practice, Agile fixes quality and the date, and manages scope.** The lever that works is **scope** (what we build), because adding people late makes things later (Brooks' Law) and cutting quality makes everything slower.

**Real-world example:**
Marketing needs an e-commerce launch on 1 March. With velocity data by mid-January, the PM sees that full scope would land ~20 March. Options discussed in January (not February 28!):
- Launch 1 March with card payments only (cut scope) ✅
- Push to 20 March (move date)
- Add a contractor for the React side (more cost; risky late)
- Skip tests (cut quality) ❌ Never.

---

### 2.3 Agile Produces Data – Velocity & Burn-down

**The single most important purpose of Agile:**

> **Destroy hope.** Replace hope with data, as early as possible, so management can manage.

Hope is the project killer. "We're 90% done" for 3 months is hope. Agile replaces it with two charts:

**1. Velocity chart** — points completed per iteration.

```
Points
 30 |        █
 25 |  █  █  █  █  █
 20 |  █  █  █  █  █  █
 15 |  █  █  █  █  █  █
    +-------------------- Iteration
       1  2  3  4  5  6
```

**2. Burn-down chart** — total points remaining over time (the line goes down as work is done, but can go up when scope is added).

```
Points left
 300 |█
 250 | █▄
 200 |   █▄
 150 |     █▄▄       ← scope added here
 100 |        █▄
  50 |          █▄
   0 +------------------ Iteration
```

From these, you can answer **"When will it be done?"** with data.

**Important rules about velocity:**
- **Points are relative**, team-specific, and only meaningful for **that** team.
- **Velocity is a measurement, not a target.** If you pressure a team to "increase velocity," they will simply inflate estimates (**Goodhart's law**).
- A **slowly declining velocity** is a sign of a **messy codebase** (or team trouble). Rising velocity without explanation is a sign of **point inflation**.
- **Never compare velocities between teams.**

#### Forecasting completion in C#

```csharp
public sealed record Iteration(int Number, int PointsCompleted);

public static class ReleaseForecaster
{
    /// Forecast how many more iterations are needed using recent average velocity ("yesterday's weather"),
    /// plus pessimistic/optimistic ranges using the min/max of recent iterations.
    public static (double Expected, double Optimistic, double Pessimistic) IterationsRemaining(
        IReadOnlyList<Iteration> history, int pointsRemaining, int window = 3)
    {
        var recent = history.TakeLast(window).Select(i => i.PointsCompleted).ToList();
        if (recent.Count == 0 || recent.Max() == 0)
            throw new InvalidOperationException("Not enough velocity data yet – run a few iterations first.");

        double avg = recent.Average();
        return (
            Expected:    pointsRemaining / avg,
            Optimistic:  pointsRemaining / (double)recent.Max(),
            Pessimistic: pointsRemaining / (double)Math.Max(1, recent.Min()));
    }
}

// Usage
var history = new List<Iteration> { new(1, 18), new(2, 24), new(3, 22), new(4, 25), new(5, 21) };
var (e, o, p) = ReleaseForecaster.IterationsRemaining(history, pointsRemaining: 140);
Console.WriteLine($"Expected ≈ {e:F1} iterations (range {o:F1}–{p:F1})");
// Expected ≈ 6.2 iterations (range 5.6–6.7)  → with 2-week iterations: ~12 weeks.
```

That's how a PM answers stakeholders: *"At current velocity, about 12 weeks; between 11 and 14 depending on the sprint. If we drop the reporting epic (35 points), it's ~9 weeks."*

---

### 2.4 The Circle of Life – XP Practices

Uncle Bob uses **Extreme Programming (XP)** as the most complete and well-defined Agile method. Its practices form three concentric rings:

```
         ┌────────────────── BUSINESS-FACING (outer) ──────────────────┐
         │   Planning Game · Small Releases · Acceptance Tests ·       │
         │   Whole Team                                                │
         │       ┌──────────── TEAM-FACING (middle) ───────────┐       │
         │       │ Sustainable Pace · Collective Ownership ·   │       │
         │       │ Continuous Integration · Metaphor           │       │
         │       │       ┌──── TECHNICAL (inner) ────┐         │       │
         │       │       │ TDD · Refactoring ·       │         │       │
         │       │       │ Simple Design · Pairing   │         │       │
         │       │       └───────────────────────────┘         │       │
         │       └─────────────────────────────────────────────┘       │
         └─────────────────────────────────────────────────────────────┘
```

**All three rings are necessary.** Business practices without technical practices = Flaccid Scrum. Technical practices without business practices = well-built software nobody asked for.

---

### 2.5 Reasons for Agile – Reasonable Expectations

**Professionalism:** Software now runs everything — cars, planes, banks, hospitals. We can't afford to be careless. Agile disciplines are what professionals do.

**Reasonable expectations** customers/managers should have of developers (and that you should live up to):

| Expectation | Meaning | How you deliver it in .NET/Azure |
|---|---|---|
| **We will not ship garbage** | Every release is high quality | Tests + code review + CI gates |
| **Continuous technical readiness** | The system is *deployable* at the end of every iteration (business decides *whether* to release) | Trunk-based development, feature flags, pipeline to staging always green |
| **Stable productivity** | Velocity doesn't decline as the project grows | Refactoring + clean architecture to keep code soft |
| **Inexpensive adaptability** | Changing requirements is cheap | Decoupled design (SOLID, Clean Architecture) |
| **Continuous improvement** | The code gets *better* over time, not worse | Boy Scout Rule, retros |
| **Fearless competence** | Developers aren't afraid to change code | Test suite you trust |
| **QA should find nothing** | Bugs are caught by the team | Automated tests at all levels |
| **Test automation** | Manual tests are expensive and unreliable | Acceptance + unit tests in CI |
| **We cover for each other** | No single points of failure | Pairing, collective ownership |
| **Honest estimates** | Ranges, not wishes | Velocity data, PERT |
| **You need to say "no"** | When things can't be done | (See *The Clean Coder*) |
| **Continuous aggressive learning** | The industry changes; keep up | Practice, katas, reading |
| **Mentoring** | Senior developers teach | Pairing, reviews |

---

### 2.6 The Bill of Rights

The goal of Agile was "to heal the divide between business and development." These rights define a healthy relationship.

#### Customer (business) rights

The customer has the right to:
1. **An overall plan** — know what can be accomplished, when, at what cost.
2. **Get the most possible value** out of every iteration.
3. **See progress** in a running system, proven to work by passing repeatable tests they specify.
4. **Change their mind**, substitute functionality, and change priorities without paying exorbitant costs.
5. **Be informed** of schedule and estimate changes, in time to choose how to reduce scope to meet a date.
6. **Cancel at any time** and be left with a useful working system reflecting investment to date.

#### Developer rights

The developer has the right to:
1. **Know what is needed**, with clear declarations of priority.
2. **Produce high-quality work** at all times.
3. **Ask for and receive help** from peers, managers, and customers.
4. **Make and update their own estimates.**
5. **Accept responsibilities** instead of having them assigned.

**Real-world interpretation for you as a future PM:**
- You **may** reprioritize anything any time (customer right #4), but you **may not** tell developers how long something takes (developer right #4), and you **may not** ask them to cut quality (developer right #2).
- Developers **must** tell you the truth early (customer right #5).

---

### 2.7 Business Practices

#### 2.7.1 Planning Game

**User Stories** are short reminders of a feature — a placeholder for a conversation, not a spec.

```
As a returning customer,
I want to reorder a previous order with one click,
so that I can save time on repeat purchases.
```

**INVEST** — a good story is:
- **I**ndependent — can be built in any order (as much as possible)
- **N**egotiable — details are discussed, not dictated
- **V**aluable — delivers value to the business
- **E**stimable — the team can estimate it
- **S**mall — fits in one iteration (ideally a few days of work for 1–2 devs)
- **T**estable — has acceptance criteria

**Estimating stories (story points):**
- Pick a **reference story** (e.g., "Login" = 3 points); estimate others **relative** to it.
- Use **Planning Poker / Flying Fingers** — everyone reveals at once.
- Too big? **Split** it. Too small? **Merge** small ones.
- Unknown? Create a **spike** — a timeboxed story to learn enough to estimate.

**Splitting example:**
`Checkout` (21 pts, too big) →
- Guest checkout with card (5)
- Saved addresses (3)
- PayPal (5)
- Discount codes (3)
- Order confirmation email (2)

**The Iteration:**
1. **Iteration Planning Meeting** (≤ ~5% of the iteration): stakeholders pick stories that fit the velocity, choosing by **ROI** (value ÷ cost). The team **doesn't** commit to finishing; they *forecast*.
2. **Yesterday's Weather:** Plan this iteration's capacity = last iteration's actual velocity. Simple and surprisingly accurate.
3. **The Midpoint check:** At halfway, ~half the points should be **done** (accepted). If not, rebalance with stakeholders now — don't wait until the end.
4. **Done means done:** a story is complete only when its **acceptance tests pass**. No partial credit. 4 of 5 tasks done = 0 points.
5. **Iteration end:** Demo to stakeholders; update velocity and burn-down charts.

**Order of work within an iteration:** Developers **should** pick stories **in priority order**, and one story at a time per pair, finishing before starting more (limit work in progress).

#### 2.7.2 Small Releases

- Release **as often as possible** — at minimum every iteration should produce a *deployable* system.
- Historically, source control/build cycles were days or weeks; with modern tools (Git, CI/CD) we can deploy many times a day.
- Shorter cycles = faster feedback = less risk.

**Azure example — continuous delivery to a staging slot every merge:**

```yaml
# .github/workflows/ci-cd.yml
name: ci-cd
on:
  push: { branches: [main] }
  pull_request: { branches: [main] }

jobs:
  build-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with: { dotnet-version: '8.0.x' }
      - run: dotnet restore
      - run: dotnet format --verify-no-changes
      - run: dotnet build -c Release --no-restore
      - run: dotnet test -c Release --no-build --logger trx
      - run: dotnet publish src/Shop.Api -c Release -o ./publish --no-build
      - uses: actions/upload-artifact@v4
        with: { name: api, path: ./publish }

  deploy-staging:
    if: github.ref == 'refs/heads/main'
    needs: build-test
    runs-on: ubuntu-latest
    environment: staging
    steps:
      - uses: actions/download-artifact@v4
        with: { name: api, path: ./publish }
      - uses: azure/login@v2
        with:
          client-id: ${{ secrets.AZURE_CLIENT_ID }}
          tenant-id: ${{ secrets.AZURE_TENANT_ID }}
          subscription-id: ${{ secrets.AZURE_SUBSCRIPTION_ID }}
      - uses: azure/webapps-deploy@v3
        with:
          app-name: app-shop-api
          slot-name: staging
          package: ./publish
```

The business decides **when** to swap staging → production. Engineering ensures it's **always possible**.

#### 2.7.3 Acceptance Tests

- **Requirements should be specified by the business as tests.**
- Written by **Business Analysts** (happy paths) and **QA** (unhappy paths, edge cases), with developers.
- Written **before** (or at the start of) the iteration — "QA should be the specifier, not the tester at the end."
- **Automated** and run in CI.
- A story is "done" when its acceptance tests pass.

```gherkin
Feature: One-click reorder

  Scenario: Reorder a previous order with all items in stock
    Given customer "anna@example.com" has a delivered order "ORD-1001" with:
      | Sku    | Qty |
      | MUG-01 | 2   |
      | TEE-M  | 1   |
    And all products are in stock
    When she reorders "ORD-1001"
    Then a new draft order is created with the same items
    And the prices are today's prices, not the original prices

  Scenario: Reorder when one item is discontinued
    Given customer "anna@example.com" has a delivered order "ORD-1002" containing discontinued "TEE-XL"
    When she reorders "ORD-1002"
    Then the new order excludes "TEE-XL"
    And she sees the message "1 item is no longer available"
```

Note how the acceptance test **surfaced business decisions** ("today's prices, not original"; "exclude discontinued, show message") that would otherwise have been guessed by a developer.

```csharp
[Binding]
public sealed class ReorderSteps(ReorderDriver driver)
{
    [When(@"she reorders ""(.*)""")]
    public Task WhenSheReorders(string orderNumber) => driver.ReorderAsync(orderNumber);

    [Then(@"the prices are today's prices, not the original prices")]
    public void ThenPricesAreCurrent() =>
        Assert.All(driver.NewOrder.Lines, l => Assert.Equal(driver.CurrentPriceOf(l.Sku), l.UnitPrice));
}
```

#### 2.7.4 Whole Team

- Formerly "On-Site Customer." The **whole team** — developers, testers, BAs, product owner — sits together (physically or virtually with high-bandwidth communication).
- The business representative is **available** to answer questions within minutes, not days.
- **Co-location** matters; remote teams can work but need deliberate effort (always-on channels, shared boards, pairing tools).

---

### 2.8 Team Practices

#### 2.8.1 Metaphor / Ubiquitous Language

The team needs a **shared vocabulary** for the system — between developers, business, and the code. Today this is what **Domain-Driven Design** calls **Ubiquitous Language**.

```csharp
// If the business says "a Policy is Bound when the Underwriter approves it":
public sealed class Policy
{
    public PolicyStatus Status { get; private set; }

    public void Bind(Underwriter approvedBy) { /* ... */ }   // ✅ business words in code
}

// ❌ not: policy.Status = 3; policyManager.UpdateFlag(p, true);
```

#### 2.8.2 Sustainable Pace

- **A software project is a marathon, not a sprint.** (Ironically, "sprint" is the Scrum term.)
- **Overtime is not a way to show dedication.** Tired developers produce messes that slow everyone down.
- **Sleep is the most precious ingredient** for a programmer.
- Occasional short overtime may be OK; continuous overtime is a management failure.

**For a PM:** If your team has worked weekends for three sprints in a row, your plan is broken. Use data (velocity) and **cut scope**, don't burn people.

#### 2.8.3 Collective Ownership

- **No one owns the code; everyone owns the code.** Anyone can improve any module.
- Avoids knowledge silos and "bus factor = 1."
- Requires: consistent standards, tests (so changes are safe), and pairing/rotation.

```
# CODEOWNERS can still be used for *review routing* – not exclusive ownership.
/src/Shop.Payments/   @shop-team
/infra/               @shop-team @platform-team
```

#### 2.8.4 Continuous Integration

- Originally: integrate every few hours. Today: **commit to main (or merge short-lived branches) at least daily**.
- **The build must never stay broken.** "Stop the presses" — if CI goes red, fixing it is the team's top priority.
- **Long-lived feature branches are the enemy** of CI (merge hell).
- Use **feature flags** to merge incomplete features safely.

```csharp
// Incomplete feature merged daily, hidden behind a flag
// (Microsoft.FeatureManagement.AspNetCore + Azure App Configuration)
app.MapPost("/api/orders/{id:guid}/reorder", async (Guid id, ReorderHandler h, CancellationToken ct) =>
        Results.Ok(await h.Handle(new ReorderCommand(id), ct)))
   .WithFeatureGate("OneClickReorder");   // returns 404 while the flag is off
```

#### 2.8.5 Stand-up Meetings

- **Optional!** Uncle Bob says teams can choose how often (daily, every other day…).
- Short: ~10 minutes for the whole team.
- Three questions: What did I do since last meeting? What will I do until the next? What's in my way?
- **Not a status report for managers.** Problems are noted, solved *after* the stand-up with the relevant people.
- Optional 4th: "Whom do you want to thank?"

---

### 2.9 Technical Practices

These are the **heart** of Agile. Without them, the outer rings fail.

#### 2.9.1 Test-Driven Development

- Analogous to **double-entry bookkeeping**: every behavior entered twice — once as a test, once as code — and they must balance.
- **Three Laws of TDD** (see *The Clean Coder*).
- Benefits: **debugging time** drops, tests serve as **documentation**, design becomes **decoupled**, and above all **courage**: you can refactor without fear.
- **Coverage:** aim very high (Uncle Bob: asymptotically approaching 100%) — but coverage is a *team* metric to spot gaps, not a management KPI to game.

#### 2.9.2 Refactoring

- **Improving the structure of code without changing its behavior** (Martin Fowler's definition).
- **Red → Green → Refactor**: the refactor step is mandatory, not optional.
- **Bigger refactorings** happen continuously, in small steps, alongside feature work — not as separate "refactoring sprints" that need business approval.

**Example — small, safe refactoring steps:**

```csharp
// BEFORE: works, all tests green, but hard to read
public decimal Fee(Order o)
{
    if (o.Total > 100 && o.Customer.Country == "DE" && !o.IsExpress) return 0;
    if (o.IsExpress) return o.Customer.Country == "DE" ? 9.90m : 19.90m;
    return o.Customer.Country == "DE" ? 4.90m : 12.90m;
}

// Step 1: Extract explaining variables (run tests)
// Step 2: Extract methods (run tests)
// Step 3: Replace conditional with lookup (run tests)

// AFTER
public decimal Fee(Order order)
{
    if (QualifiesForFreeShipping(order)) return 0m;
    return ShippingRates.For(order.Customer.Country, order.IsExpress);
}

private const decimal FreeShippingThreshold = 100m;

private static bool QualifiesForFreeShipping(Order o) =>
    o.Total > FreeShippingThreshold && o.Customer.Country == "DE" && !o.IsExpress;

private static class ShippingRates
{
    private static readonly Dictionary<(bool Domestic, bool Express), decimal> Rates = new()
    {
        [(true,  false)] = 4.90m,
        [(true,  true )] = 9.90m,
        [(false, false)] = 12.90m,
        [(false, true )] = 19.90m,
    };

    public static decimal For(string country, bool express) => Rates[(country == "DE", express)];
}
```

Behavior identical; tests green after every step. That's refactoring.

#### 2.9.3 Simple Design

Kent Beck's rules (in priority order):
1. **Pass all the tests.**
2. **Reveal the intent.**
3. **Remove duplication.**
4. **Decrease elements** (fewest classes, methods, variables).

**Design weight:** Every abstraction has a cost. Don't add an interface, a factory, a pattern "in case we need it." Add it when the tests/requirements demand it. (**YAGNI** — You Aren't Gonna Need It.)

#### 2.9.4 Pair Programming

- Two people, one problem (one keyboard or remote with Live Share / VS Code / Rider Code With Me).
- **Optional and intermittent** — not mandated 100% of the time. Pair on hard problems, new areas, onboarding.
- **Benefits:** knowledge sharing, fewer defects, continuous code review, collective ownership, mentoring.
- **Is it wasteful?** Studies suggest pairs are ~15% slower in raw output but produce significantly fewer defects — net win.
- **Mob programming** (whole team, one problem) is a variant.
- **Pairing ≠ "two people doing one person's job."** It's real-time review + design.

Styles:
- **Driver/Navigator:** one types, one thinks ahead.
- **Ping-Pong:** A writes a failing test, B makes it pass and writes the next failing test, swap.

---

### 2.10 Becoming Agile

#### Agile values (from XP)
- **Courage** — to make reasonable risks, to refactor, to say no, to be honest about progress.
- **Communication** — face-to-face, informal, frequent.
- **Feedback** — short cycles, charts, tests, CI.
- **Simplicity** — the simplest thing that could possibly work; directness (no hidden agendas).

#### The menagerie (frameworks)
Scrum, XP, Kanban, Crystal, FDD, DSDM, SAFe… Uncle Bob's view: **XP is the most complete** because it includes technical practices. Many organizations adopt **Scrum's** business practices **and XP's** technical practices — a good combination.

#### Agile transformation
- Agile **values** often conflict with the values of **middle management** (control, predictability, status). That's why transformations often fail at the middle.
- Sometimes teams adopt Agile successfully "underground" while still producing the reports management expects.

#### Coaching
- Coaches **teach practices and values**, don't manage. They should eventually make themselves unnecessary.
- A coach ≠ a Scrum Master who acts as project manager.

#### Certification
- Uncle Bob: **Agile certifications are mostly worthless** as proof of skill (a 2-day course doesn't make anyone a master). Training is useful; the certificate isn't the point.
- (Practical note for your career: some employers filter on certifications like PSM/CSM/AZ-400; it doesn't hurt to have them, but **skills** are what make you top 1%.)

#### Agile in the large
- Agile is about **small teams**. Large organizations = many small teams. The problem of coordinating them is an old management problem, not an Agile problem.
- Frameworks like SAFe/LeSS try to solve this; be pragmatic and keep the **team-level disciplines** intact.

#### Tools
- **Start low-tech.** Index cards and a whiteboard teach the process better than any tool.
- Tools should **serve** the process, not dictate it. Adopt Jira/Azure Boards *after* you know what you need.
- Good tools: **Git**, a good IDE, **CI server**, automated testing frameworks. These are non-negotiable.

---

### 2.11 Craftsmanship

(Guest chapter by **Sandro Mancuso**.)

- Agile's business side won; its technical side was neglected → "**Agile hangover**": teams doing ceremonies, producing poor code.
- The **Software Craftsmanship Manifesto (2009)** extended the Agile Manifesto:
  - Not only working software, but also **well-crafted software**
  - Not only responding to change, but also **steadily adding value**
  - Not only individuals and interactions, but also **a community of professionals**
  - Not only customer collaboration, but also **productive partnerships**
- **Craftsmanship is about professionalism**: doing the job well, taking pride, and continuously learning.
- **Practices are not ideology.** Use them because they produce value; drop them if better ones come along. Discuss practices in terms of **value**, not dogma.
- Craftsmen **mentor** others and build a community of practice (meetups, katas, code retreats).

---

## 3. A Full Iteration – Walkthrough for a .NET + React + Azure Team

**Team:** 4 devs (.NET + React), 1 QA, 1 PO, 1 BA. 2-week iterations. Velocity last 3 iterations: 21, 24, 22 → **plan for ~22**.

### Day 1 – Planning (~2 hours max)

| Story | Points | Business value | ROI order |
|---|---|---|---|
| One-click reorder (API + React button) | 8 | High | 1 |
| Show estimated delivery date at checkout | 5 | High | 2 |
| Export orders to CSV for Finance | 3 | Medium | 3 |
| Admin: deactivate products | 5 | Medium | 4 |
| Spike: Cosmos DB for product catalog (timeboxed 2 days) | 2 | — | 5 |
| **Total** | **23** | | |

PO knows the last item may slip. Acceptance tests (Gherkin) for stories 1–2 are already written by BA + QA.

### Days 2–5 – Work

- Pairs pick the **top story first**. TDD on the domain and use case; React component with Vitest + React Testing Library.
- Merge to `main` at least once a day, behind feature flags.
- CI runs unit + acceptance tests; deploys to the **staging slot**.
- Stand-up: 10 minutes. Blockers solved afterwards.

### Day 6 – Midpoint check

- Expected ~11 points **done** (acceptance tests passing). Actual: 8.
- PO + team agree: the spike moves to next iteration. No overtime.

### Days 7–9 – Work

- QA does exploratory testing on staging; finds nothing critical (goal: QA finds nothing).
- Refactoring done continuously — e.g., extracted `DeliveryDateCalculator` from the checkout handler.

### Day 10 – Demo & Retrospective

- Demo: running software on staging, not slides.
- Velocity recorded: 21 (story 4 not finished → **0 points** for it, no partial credit).
- Burn-down updated → release forecast refreshed for stakeholders.
- Retro (~30–45 min): one or two concrete improvements, e.g., "flaky Playwright test – fix it or delete it this iteration."
- PO decides whether to **swap staging → production**. The system is ready either way.

---

## 4. For Your Future Project Manager Role

Uncle Bob's advice condensed into a PM playbook:

| Do | Don't |
|---|---|
| Manage **scope** using velocity data | Demand all of Good/Fast/Cheap/Done |
| Ask for **estimates as ranges**; let devs own them | Turn estimates into commitments or targets |
| Reprioritize freely between iterations | Change scope mid-iteration without negotiation |
| Watch **velocity trends** as a code-health signal | Compare velocities across teams or reward "higher velocity" |
| Protect **sustainable pace** | Treat overtime as normal |
| Insist on **automated acceptance tests** as "done" | Accept "done but not tested" |
| Keep teams **stable** and feed them work | Shuffle people across projects |
| Make **continuous deployability** a requirement | Allow long-lived branches and "integration phases" |
| Value **working software demos** | Rely on status reports and % complete |
| Fund **technical practices** (TDD, CI, refactoring) as part of the job | Ask "can we skip tests to go faster?" |

**Key PM sentence to remember:**
> "Agile won't make us faster. It will tell us — early and honestly — how fast we are, so we can make good decisions about scope."

---

## 5. Planning Checklist

**Before the project**
- [ ] Is there a **CI/CD pipeline** on day 1 (build, test, deploy to staging)?
- [ ] Do we have a **Definition of Done** that includes automated acceptance tests?
- [ ] Is there a **product owner** available daily?
- [ ] Are stories written with **INVEST** and acceptance criteria?
- [ ] Are we tracking **velocity** and a **burn-down/burn-up** chart?

**Each iteration**
- [ ] Capacity = yesterday's weather (last velocity).
- [ ] Stories ordered by ROI; WIP limited.
- [ ] Midpoint check done; scope adjusted early if needed.
- [ ] Every story merged daily; feature flags for incomplete work.
- [ ] Demo of working software; system deployable.
- [ ] Retrospective with 1–2 concrete actions.

**Ongoing**
- [ ] Velocity stable (not declining) → code is staying clean.
- [ ] No sustained overtime.
- [ ] Refactoring happening continuously, not in special "tech debt sprints."
- [ ] Pairing on hard/unknown areas; knowledge spread across team.

---

## 6. Interview Questions & Answers

**Q1. What is Agile, in your own words?**
A feedback-driven approach where small teams deliver working software in short iterations, producing data (velocity, burn-down) that lets the business make informed decisions early. It requires business, team, and technical practices together.

**Q2. What's the Iron Cross?**
Good, Fast, Cheap, Done — you can't maximize all four. Agile fixes quality, uses data to manage scope (and sometimes date/cost), and never cuts quality.

**Q3. What is velocity and how should it be used?**
Story points completed per iteration. It's a measurement for forecasting ("yesterday's weather"), not a performance target. Never compare teams by velocity; a declining trend signals code or team health issues.

**Q4. What does "done" mean in an Agile team?**
All acceptance tests pass, code is reviewed and merged, the system is deployable. There's no partial credit — an unfinished story is worth 0 points this iteration.

**Q5. What are the XP technical practices, and why do they matter?**
TDD, Refactoring, Simple Design, Pair Programming. Without them, the codebase decays, velocity drops, and Agile's promises (stable productivity, cheap change) fail — so-called "Flaccid Scrum."

**Q6. What's a spike?**
A timeboxed story used to research something unknown enough to estimate it (e.g., "evaluate Cosmos DB for catalog – 2 days").

**Q7. What's INVEST?**
Independent, Negotiable, Valuable, Estimable, Small, Testable — the qualities of a good user story.

**Q8. How do you handle a stakeholder asking for more in the same time?**
Show the velocity and burn-down data, explain the Iron Cross, and offer options: reduce scope, move the date, or (early enough) add capacity — but not reduce quality.

**Q9. What is Continuous Integration really?**
Everyone integrates into the mainline at least daily, every integration is verified by an automated build and tests, and a broken build is fixed immediately. Long-lived branches defeat it; feature flags enable it.

**Q10. What's the role of QA in Agile?**
QA writes acceptance tests up front (as specifiers) and does exploratory testing. The development process should be good enough that QA finds nothing.

**Q11. Is Scrum the same as Agile?**
Scrum is one Agile framework focusing on business/team practices. Agile (as originally intended, via XP) also requires technical practices. Many effective teams combine Scrum with XP engineering practices.

**Q12. What's the Customer and Developer Bill of Rights?**
Customers: a plan, max value per iteration, see progress, change their mind, be informed early, cancel with a working system. Developers: know what's needed, produce quality work, get help, own their estimates, accept (not be assigned) responsibilities.

---

## 7. Key Takeaways

1. **Agile is small:** small teams, small iterations, small releases.
2. **Agile's main product is data**, which destroys hope and enables management.
3. **Good, Fast, Cheap, Done — pick three.** Manage scope; never cut quality.
4. **Velocity is a measurement, never a target.**
5. **Done means acceptance tests pass.** No partial credit.
6. **The system must be deployable at the end of every iteration.**
7. **The Bill of Rights** balances business and developers.
8. **Sustainable pace** — a marathon, not a sprint.
9. **Collective ownership + CI + pairing** = no silos, no merge hell.
10. **TDD, Refactoring, Simple Design, Pairing** are the engine; without them Agile fails.
11. **Certifications ≠ skill.** Craftsmanship = professionalism + continuous learning.
12. **Start simple** — cards and a whiteboard before tools.

---

*Previous: [03 – Clean Architecture](03-Clean-Architecture.md) · Next: [05 – Agile Software Development: Principles, Patterns, and Practices](05-Agile-Software-Development-PPP.md)*
