# Clean Craftsmanship: Disciplines, Standards, and Ethics

> **Author:** Robert C. Martin ("Uncle Bob") · **Published:** 2021
> **One-line summary:** Software now runs the world, so programmers need what doctors and engineers have: **disciplines** (how we work), **standards** (what others can expect from us), and **ethics** (the promises we make).
> **Examples in this guide:** C# 12 / .NET 8+, xUnit, NSubstitute, Stryker.NET.

---

## Table of Contents

1. [What the Book Actually Covers](#1-what-the-book-actually-covers)
2. [Part I – The Disciplines](#2-part-i--the-disciplines)
   - 2.1 [Test-Driven Development: the Basics](#21-test-driven-development-the-basics)
   - 2.2 [Advanced TDD: the Transformation Priority Premise](#22-advanced-tdd-the-transformation-priority-premise)
   - 2.3 [Test Doubles](#23-test-doubles)
   - 2.4 [Test Design](#24-test-design)
   - 2.5 [Refactoring](#25-refactoring)
   - 2.6 [Simple Design](#26-simple-design)
   - 2.7 [Collaborative Programming](#27-collaborative-programming)
   - 2.8 [Acceptance Tests](#28-acceptance-tests)
3. [Part II – The Standards](#3-part-ii--the-standards)
4. [Part III – The Ethics: The Programmer's Oath](#4-part-iii--the-ethics-the-programmers-oath)
5. [Putting It Together in a .NET + React + Azure Team](#5-putting-it-together-in-a-net--react--azure-team)
6. [Checklist](#6-checklist)
7. [Interview Questions & Answers](#7-interview-questions--answers)
8. [Key Takeaways](#8-key-takeaways)

---

## 1. What the Book Actually Covers

This book combines and extends ideas from *Clean Code*, *The Clean Coder*, and *Clean Agile*. Its big argument:

> **Programmers rule the world.** Software is in cars, planes, medical devices, voting machines, and banks. Sooner or later software will cause a major disaster, and society will ask "how could you let this happen?" Programmers need a professional code of conduct **before** someone else imposes one.

| Part | Topic | What you learn |
|---|---|---|
| **I. The Disciplines** | TDD, Test Design, Refactoring, Simple Design, Collaborative Programming, Acceptance Tests | *How* a craftsman works, minute by minute. This is the largest part, with long TDD walkthroughs (stack, prime factors, sorting, bowling, video store). |
| **II. The Standards** | Productivity, Quality, Courage | What a business (or "your new CTO") has a right to expect from programmers. |
| **III. The Ethics** | The Programmer's Oath | Nine promises a professional programmer makes. |

**What's new compared with earlier books:**
- A much deeper look at **TDD** (test doubles, the Transformation Priority Premise, mutation testing, testing GUIs and databases).
- The **Programmer's Oath**.
- The focus moves from personal professionalism to **responsibility to society**.

---

## 2. Part I – The Disciplines

A **discipline** is a set of rules you follow *even when you don't feel like it*, because you know the result is better. Surgeons scrub in every time. Pilots run the checklist every time.

### 2.1 Test-Driven Development: the Basics

#### The Three Laws (recap)

1. Don't write production code until you have a failing test.
2. Don't write more of a test than is enough to fail (not compiling counts as failing).
3. Don't write more production code than is enough to pass the failing test.

#### The fourth law: Refactor

Uncle Bob adds **Red → Green → Refactor** as a discipline of its own. "Make it work, then make it right." Never skip the refactor step.

#### What TDD gives you

| Benefit | Why |
|---|---|
| **Less debugging** | You're always seconds away from the last green state. |
| **Documentation** | Tests are small, exact examples of how to use each part of the system. |
| **Fun** | Writing tests *after* feels like a chore; writing them first feels like solving a puzzle. |
| **Completeness** | If every line was written to pass a test, almost every line is covered. |
| **Design** | Testable code is decoupled code. |
| **Courage** | You trust the suite, so you dare to clean the code. |

**Real-world analogy:** Double-entry bookkeeping. Accountants record every transaction twice, in two accounts, and the books must balance. TDD records every behavior twice (test + code), and they must agree.

#### The "Stack" kata – how the book teaches TDD

Each test forces one small step:

```csharp
public class StackTests
{
    private readonly Stack<int> _stack = new();  // our own Stack, not System.Collections.Generic

    [Fact] public void NewStack_IsEmpty() => Assert.True(_stack.IsEmpty);

    [Fact] public void AfterOnePush_IsNotEmpty()
    {
        _stack.Push(0);
        Assert.False(_stack.IsEmpty);
    }

    [Fact] public void PopOnEmptyStack_Throws() =>
        Assert.Throws<UnderflowException>(() => _stack.Pop());

    [Fact] public void AfterPushingX_PopReturnsX()
    {
        _stack.Push(99);
        Assert.Equal(99, _stack.Pop());
    }

    [Fact] public void AfterPushingXThenY_PopReturnsYThenX()
    {
        _stack.Push(1); _stack.Push(2);
        Assert.Equal(2, _stack.Pop());
        Assert.Equal(1, _stack.Pop());
    }
}

public sealed class UnderflowException : Exception;

public sealed class Stack<T>
{
    private readonly List<T> _elements = [];
    public bool IsEmpty => _elements.Count == 0;
    public void Push(T item) => _elements.Add(item);

    public T Pop()
    {
        if (IsEmpty) throw new UnderflowException();
        var top = _elements[^1];
        _elements.RemoveAt(_elements.Count - 1);
        return top;
    }
}
```

The *first* version of `IsEmpty` was `=> true`. That looks silly, but it's the point: **write only what the tests demand**, and let the next test force you to generalize.

> **Golden rule of TDD:** *As the tests get more specific, the code gets more generic.*

---

### 2.2 Advanced TDD: the Transformation Priority Premise

When a test fails, you change the production code. Uncle Bob noticed that these changes come in a small set of **transformations**. Prefer the simpler ones (higher in the list):

| # | Transformation | Example |
|---|---|---|
| 1 | {} → Nil | No code → `return null;` / `return [];` |
| 2 | Nil → Constant | `return [];` → `return [2];` |
| 3 | Constant → Variable | `return [2];` → `return [n];` |
| 4 | Add computation | `return n * 2;` |
| 5 | Split flow (Unconditional → If) | add an `if` |
| 6 | Variable → Array | single value → list |
| 7 | Array → Container | list → dictionary/tree |
| 8 | If → While | `if` becomes a loop |
| 9 | Statement → Recursion | |
| 10 | Value → Mutated value | assignment |

**Why it matters:** If you pick the **simplest transformation** that passes each test, you're less likely to get **stuck** (a state where the next test needs a huge rewrite). It's also why the Prime Factors kata turns a chain of `if`s into two nested `for` loops in such a tidy way.

**Getting stuck** = a test needs a big jump. Back out, and write a simpler test first.

**Real-world example – a discount rule built by transformations:**

```csharp
// Test 1: no items → 0                      Transformation: {} → constant
public decimal Discount(Cart c) => 0m;

// Test 2: total ≥ 100 → 10% off             Transformation: unconditional → if
public decimal Discount(Cart c) => c.Total >= 100 ? c.Total * 0.10m : 0m;

// Test 3: VIP → 15% at any total            Transformation: add computation / split flow
public decimal Discount(Cart c) => c.Total * Rate(c);
private static decimal Rate(Cart c) => c.IsVip ? 0.15m : c.Total >= 100 ? 0.10m : 0m;
```

#### Test-writing patterns from the book

- **Arrange / Act / Assert** (also *Given / When / Then* in BDD). Each test has exactly these three parts.
- **One logical assert** per test concept.
- **Finite-state-machine view:** each test is a transition `Given state X, When event E, Then state Y`. If you list every state and event, you know which tests are missing.

---

### 2.3 Test Doubles

The book clarifies the names (from Gerard Meszaros), which developers mix up all the time:

| Double | What it does | Use it when… |
|---|---|---|
| **Dummy** | Passed in but never used; methods do nothing or throw | You need to satisfy a parameter |
| **Stub** | Returns canned values | You need to drive the code down a specific path |
| **Spy** | A stub that also **records** how it was called | You need to check that a call happened |
| **Mock** | A spy that **knows what to expect** and verifies itself | You test interactions (use sparingly) |
| **Fake** | A working, simplified implementation (in-memory DB) | You need realistic behavior without infrastructure |

```csharp
public interface IAuthenticator { bool Authenticate(string user, string password); }

// Dummy – LoginDialog needs one, but this test never logs in
public sealed class DummyAuthenticator : IAuthenticator
{
    public bool Authenticate(string u, string p) => throw new InvalidOperationException("Should not be called");
}

// Stub – forces the "rejected" path
public sealed class RejectingAuthenticatorStub : IAuthenticator
{
    public bool Authenticate(string u, string p) => false;
}

// Spy – records calls
public sealed class AuthenticatorSpy : IAuthenticator
{
    public int Count { get; private set; }
    public string? LastUser { get; private set; }
    public bool Result { get; set; }
    public bool Authenticate(string u, string p) { Count++; LastUser = u; return Result; }
}

// Fake – realistic but in-memory
public sealed class InMemoryAuthenticator(Dictionary<string, string> users) : IAuthenticator
{
    public bool Authenticate(string u, string p) => users.TryGetValue(u, out var pw) && pw == p;
}
```

With a library (NSubstitute) the same ideas are shorter:

```csharp
var auth = Substitute.For<IAuthenticator>();
auth.Authenticate("bob", "secret").Returns(true);          // stub
var dialog = new LoginDialog(auth);
dialog.Submit("bob", "secret");
auth.Received(1).Authenticate("bob", "secret");             // spy/mock verification
```

**Uncle Bob's advice:** prefer hand-written stubs/spies/fakes for clarity; reach for mocking frameworks when hand-writing gets tedious. **Don't over-mock**: tests that verify every internal call are fragile.

#### The "Uncertainty Principle" – London vs Chicago (Detroit) school

- **London school (mockist):** test each class in isolation with mocks; verify interactions. Precise, but couples tests to implementation.
- **Chicago school (classicist):** test behavior through real objects; verify results/state. More robust; failures are less precisely located.

Uncle Bob says both are valid and he leans **classicist**, using doubles mainly at **architectural boundaries** (DB, UI, network).

---

### 2.4 Test Design

- **The Fragile Test Problem:** when tests are coupled to the structure of production code, every small refactor breaks dozens of tests, and the team starts to hate them. **Tests should be coupled to behavior, not structure.**
- **Don't mirror production classes one-to-one** with test classes. Test through stable APIs (use cases) so internals can move freely.
- **Testing databases:** don't test business rules through the DB. Put a gateway interface between them; test the gateway separately with a real DB (Testcontainers).
- **Testing GUIs:** use **Humble Object**. Put all logic in a presenter, test the presenter, and keep the view too simple to need tests.
- **Test patterns:** *Test-Specific Subclass* (override a method to sense or control it), *Self-Shunt* (the test class itself implements the interface as a spy), *Humble Object*.
- **Mutation testing:** change the production code deliberately (flip `>` to `>=`, delete a line) and check that a test fails. If no test fails, you have a coverage hole that line coverage can't show.

```bash
# Mutation testing for .NET
dotnet tool install -g dotnet-stryker
cd tests/Shop.Domain.Tests
dotnet stryker
# Report: "Mutation score 87%". Survived mutants show exactly where tests are weak.
```

- **Test speed:** the unit-test suite should run in **seconds**. Slow suites stop being run.

```csharp
// Self-Shunt: the test class is its own spy
public class AlarmTests : IAlarmSounder
{
    private int _alarms;
    public void Sound() => _alarms++;

    [Fact]
    public void DoorLeftOpen_SoundsAlarm()
    {
        var door = new TimedDoor(alarm: this);
        door.Open();
        door.TimeOut();
        Assert.Equal(1, _alarms);
    }
}
```

---

### 2.5 Refactoring

> **Refactoring** is changing the structure of code without changing its behavior, in small steps, with the tests passing after every step.

- **Red → Green → Refactor**: refactoring is part of every cycle, not a separate project.
- **Bigger refactorings** are done as a sequence of tiny ones over days or weeks, while features keep shipping.
- **The basic toolkit** (learn the IDE shortcuts):
  - **Rename** (the most important refactoring of all)
  - **Extract Method**
  - **Extract Variable**
  - **Extract Field**
  - **Inline**
  - **Move Method / Move Class**
- **The Rubik's Cube:** Uncle Bob compares refactoring moves to cube algorithms. Master a few moves and you can reach almost any structure.
- **The disciplines:** run the tests after **every** change; if a change breaks tests, undo it. **Never refactor without tests.** On legacy code, first write *characterization tests* that pin current behavior.

```csharp
// Characterization test – documents what legacy code DOES today (even if it's odd)
[Fact]
public void LegacyInvoiceNumber_Format_IsPinned()
{
    var number = LegacyInvoiceService.NextNumber(year: 2026, sequence: 7);
    Assert.Equal("INV-26-0007", number);  // whatever it returns today, freeze it before refactoring
}
```

---

### 2.6 Simple Design

Kent Beck's rules, in Uncle Bob's wording. Code is simple when it is:

1. **Covered by tests.**
2. **Maximally expressive:** names and structure reveal intent.
3. **Free of duplication.**
4. **Minimal:** the smallest number of classes, methods, and lines that satisfies 1–3.

Plus **YAGNI** ("You Aren't Gonna Need It"): don't build hooks for imagined futures. Uncle Bob's nuance: in the old days, refactoring was expensive, so people predicted the future. With tests, change is cheap, so you can afford to wait until the need is real.

**Design weight:** every abstraction costs something to understand. A pattern should reduce total weight, not add to it.

```csharp
// ❌ YAGNI violation: one exporter, but a plugin framework "just in case"
public interface IExporterPlugin { string Name { get; } bool CanHandle(ExportFormat f); Task ExportAsync(...); }
public sealed class ExporterPluginRegistry { /* reflection-based discovery, 200 lines */ }

// ✅ What's actually needed today
public sealed class CsvOrderExporter { public string Export(IEnumerable<Order> orders) { ... } }
// When a second format really arrives, extract the interface then. With tests, that takes ~10 minutes.
```

---

### 2.7 Collaborative Programming

- **Pair programming** and **mob (ensemble) programming** are the best ways to spread knowledge, review code in real time, and avoid silos.
- They are **not mandatory 100% of the time**. Pair when the problem is hard, the area is unfamiliar, or someone is learning.
- Pairing styles: Driver/Navigator, Ping-Pong TDD (one writes the test, the other makes it pass), Strong-style (the idea must go through someone else's hands).
- **Managers:** pairing is not "half the output". The goal is fewer defects, shared ownership, and faster onboarding.

**Remote tooling:** VS Live Share, Rider Code With Me, VS Code + Teams screen share, `git mob` co-author trailers.

---

### 2.8 Acceptance Tests

- Requirements should be written as **automated acceptance tests** by the business (BAs, QA) together with developers.
- They define "done" and run in CI.
- They test **business rules through the API or use-case layer**, not through the UI.
- Developers must **never** change an acceptance test's meaning without the business agreeing.

```gherkin
Scenario: Late return fee
  Given a member borrowed "Clean Craftsmanship" due on 2026-11-10
  When the book is returned on 2026-11-13
  Then the member is charged a late fee of 1.50 EUR
```

*(See [02 – The Clean Coder](02-The-Clean-Coder.md) and [04 – Clean Agile](04-Clean-Agile.md) for full Reqnroll examples.)*

---

## 3. Part II – The Standards

Uncle Bob frames this as **"your new CTO"** telling the team what they expect. These are the standards any business has a right to expect from professional programmers.

### 3.1 Productivity

| Standard | Meaning | .NET / Azure practice |
|---|---|---|
| **We will never ship bad code** | Every release is defect-free and clean | Tests, code review, analyzers, CI gates |
| **Continuous readiness** | The system can be deployed at any moment | Trunk-based dev, feature flags, always-green pipeline to a staging slot |
| **Stable productivity** | Velocity does not decay as the system grows | Continuous refactoring, Clean Architecture |

### 3.2 Quality

| Standard | Meaning |
|---|---|
| **Continuous improvement** | The code gets better over time, never worse (Boy Scout Rule) |
| **Fearless competence** | Nobody is afraid to change any code, because the tests protect them |
| **Extreme quality** | Defects are rare and surprising |
| **We will not dump on QA** | QA is not the bug-finding safety net |
| **QA will find nothing** | Bugs found by QA mean the dev process has a gap to fix |
| **Test automation** | Manual testing is too slow and unreliable; machines run tests |

### 3.3 Courage

| Standard | Meaning |
|---|---|
| **We cover for each other** | Any team member can pick up any work (pairing, collective ownership) |
| **Honest estimates** | Ranges with probabilities; no wishful thinking |
| **You must say no** | When something can't be done well, say so |
| **Continuous aggressive learning** | Keep up with an industry that changes every few years |
| **Mentoring** | Seniors teach juniors; that's how the craft survives |

**For your future PM role:** these standards are a ready-made "team charter". Put them on the wiki, and use them in hiring, onboarding, and retrospectives.

---

## 4. Part III – The Ethics: The Programmer's Oath

Uncle Bob proposes an oath, similar in spirit to the Hippocratic Oath for doctors. In summary, a programmer promises, to the best of their ability and judgment:

| # | Promise (paraphrased) | In practice |
|---|---|---|
| 1 | **Do no harm.** Don't write code that harms users, society, or the business. | Refuse to build deceptive dark patterns, emissions-cheating code, or features that knowingly endanger people. Think about security and privacy (GDPR, OWASP). |
| 2 | **Always do your best work.** Don't let defective behavior or messy structure pile up. | Clean code, refactoring, no "I'll fix it later" that never happens. |
| 3 | **Provide proof.** Each release comes with a quick, sure, repeatable proof that the code works. | An automated test suite that runs in CI on every change. |
| 4 | **Small, frequent releases** so you don't block others. | Short-lived branches, merge daily, small PRs. |
| 5 | **Relentlessly improve** your creations; never degrade them. | Boy Scout Rule, continuous refactoring. |
| 6 | **Keep productivity high**, yours and others'. | Fast builds, fast tests, no broken builds, good tooling, no messes left for others. |
| 7 | **Make sure others can cover for you**, and you for them. | Pairing, documentation of *why*, no knowledge silos. |
| 8 | **Estimate honestly**, in size and precision; don't promise without certainty. | PERT ranges, "I will … by …" only when sure. |
| 9 | **Never stop learning** and improving your craft. | Katas, reading, new languages, mentoring. |

### "Harm" in more detail

The book splits harm into kinds:
- **Harm to society:** e.g., the Volkswagen emissions scandal, where programmers wrote code to cheat tests. "I was told to" is not a defense.
- **Harm to function:** shipping bugs. You can't promise perfection, but you can promise you **did everything reasonable** (tests, reviews) to prevent defects.
- **Harm to structure:** making code rigid and messy, which hurts everyone who works on it later. **Software must stay *soft*.**

**Real-world example:**
A product manager asks you to make the "Cancel subscription" button tiny, grey, and hidden behind four screens. That is harm to users. The professional response: raise the concern, explain the legal/reputational risk (consumer-protection laws), propose an honest flow, and escalate if needed.

---

## 5. Putting It Together in a .NET + React + Azure Team

| Discipline / Standard | Concrete setup |
|---|---|
| TDD | xUnit + NSubstitute for .NET; Vitest + React Testing Library for React; tests run on save (`dotnet watch test`, Vitest watch mode) |
| Mutation testing | Stryker.NET in a nightly pipeline; StrykerJS for React |
| Acceptance tests | Reqnroll against `WebApplicationFactory<Program>` |
| Refactoring | Rider / Visual Studio refactorings; `dotnet format`; analyzers as errors |
| Simple design | PR checklist item: "Is there anything here we don't need yet?" |
| Collaborative programming | Pair on every story estimated > 5 points; weekly mob session for hard problems |
| Continuous readiness | GitHub Actions / Azure Pipelines → staging slot on every merge; slot swap to production |
| Quick, repeatable proof | Branch protection: required checks (build, tests, coverage threshold, Stryker score) |
| Cover for each other | CODEOWNERS for *review routing* only; rotate on-call; ADRs (Architecture Decision Records) in the repo |
| Do no harm | Threat modeling, OWASP checks, dependency scanning (Dependabot / GitHub Advanced Security), privacy review for PII in Azure |

```yaml
# Nightly mutation testing – proof that the tests really test something
- name: Mutation testing
  run: |
    dotnet tool restore
    dotnet stryker --project src/Shop.Domain/Shop.Domain.csproj --break-at 75
```

---

## 6. Checklist

**Every coding session**
- [ ] I wrote a failing test before each piece of behavior.
- [ ] I chose the simplest transformation to make it pass.
- [ ] I refactored while green.
- [ ] My tests check behavior, not internal structure.
- [ ] The full unit suite still runs in seconds.

**Every PR**
- [ ] Nothing speculative (YAGNI).
- [ ] Doubles used only at boundaries; no over-mocking.
- [ ] Acceptance tests for the story pass.
- [ ] Code is cleaner than before.

**Every week**
- [ ] I paired or mobbed at least once.
- [ ] I practiced a kata.
- [ ] I helped someone learn something.

**Ethics**
- [ ] Nothing I shipped this week could harm users, society, or the codebase.

---

## 7. Interview Questions & Answers

**Q1. What is the difference between a stub, a spy, a mock, and a fake?**
A stub returns canned answers. A spy is a stub that records calls. A mock is a spy that knows the expected calls and verifies them itself. A fake is a working, lightweight implementation (like an in-memory repository). A dummy is just a placeholder that's never used.

**Q2. What is the Transformation Priority Premise?**
An ordered list of code transformations (nil → constant → variable → if → while → recursion …). When making a failing test pass, choose the simplest transformation. It keeps TDD steps small and helps you avoid getting stuck.

**Q3. "As the tests get more specific, the code gets more generic." What does that mean?**
Each new test adds a specific case. Instead of adding a special `if` for each case, the production code should become a more general algorithm that handles all cases.

**Q4. What is mutation testing and why use it?**
A tool changes the production code (for example, flips a condition). If no test fails, that "mutant" survived, which reveals a gap. It measures how good the tests are, which line coverage can't do.

**Q5. What is the fragile test problem and how do you avoid it?**
Tests break whenever the code's structure changes, even though behavior hasn't. Avoid it by testing through stable APIs, not mirroring the class structure, and only using mocks at architectural boundaries.

**Q6. London vs Chicago school of TDD?**
London (mockist) isolates each class with mocks and checks interactions. Chicago (classicist) uses real collaborators and checks results. Classicist tests survive refactoring better; mockist tests locate failures more precisely. Most teams mix them, with mocks at boundaries.

**Q7. What is the Programmer's Oath?**
Uncle Bob's nine promises: do no harm, do your best work, provide repeatable proof the code works, release small and often, keep improving the code, keep productivity high, make sure others can cover for you, estimate honestly, and never stop learning.

**Q8. A manager asks you to implement something unethical or harmful. What do you do?**
Raise the concern clearly with the reasons (user harm, legal risk, reputation), suggest an alternative, and escalate if needed. "I was told to" doesn't remove a programmer's responsibility. The Volkswagen emissions case is the classic example.

**Q9. How do you refactor legacy code safely?**
First write characterization tests that pin down current behavior. Then refactor in tiny steps using IDE refactorings, running tests after each one.

**Q10. What are the four rules of simple design?**
Covered by tests, maximally expressive, no duplication, minimal size, in that priority order.

---

## 8. Key Takeaways

1. **Software runs the world**, so programmers need disciplines, standards, and ethics.
2. **TDD is the core discipline**: three laws, plus refactor every cycle.
3. **Tests get specific, code gets generic.** Choose the simplest transformation.
4. **Know your test doubles** and use mocks mostly at boundaries.
5. **Couple tests to behavior, not structure.**
6. **Mutation testing** shows whether your tests actually test anything.
7. **Refactor in tiny steps**, always green.
8. **Simple design + YAGNI**: tests make it cheap to wait.
9. **Collaborate**: pair and mob to share knowledge and quality.
10. **Standards:** never ship bad code, always be ready to deploy, QA finds nothing, cover for each other.
11. **The Oath:** do no harm, prove it works, estimate honestly, keep learning.

---

*Previous: [05 – Agile PPP](05-Agile-Software-Development-PPP.md) · Next: [07 – Functional Design](07-Functional-Design.md)*
