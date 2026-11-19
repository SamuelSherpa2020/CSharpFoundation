# Robert C. Martin ("Uncle Bob") – Study Guides for .NET Full-Stack Developers

Eight plain-language summaries of Uncle Bob's main books, with examples in **C# / .NET 8+** (plus React and Azure where they help). Use them for interview prep, for planning before you build, and as a reference while you work.

| # | File | Book | Level it works at | Read it when… |
|---|---|---|---|---|
| 01 | [01-Clean-Code.md](01-Clean-Code.md) | *Clean Code* (2008) | Lines, functions, classes | You're writing or reviewing code every day |
| 02 | [02-The-Clean-Coder.md](02-The-Clean-Coder.md) | *The Clean Coder* (2011) | You as a professional | Estimating, committing, under pressure, behavioral interviews |
| 03 | [03-Clean-Architecture.md](03-Clean-Architecture.md) | *Clean Architecture* (2017) | Components and whole systems | Designing a new solution or service |
| 04 | [04-Clean-Agile.md](04-Clean-Agile.md) | *Clean Agile* (2019) | Team and process | Planning iterations, leading a team, PM work |
| 05 | [05-Agile-Software-Development-PPP.md](05-Agile-Software-Development-PPP.md) | *Agile Software Development: Principles, Patterns, and Practices* (2002) | SOLID and design patterns | Going deep on SOLID and patterns, design interviews |
| 06 | [06-Clean-Craftsmanship.md](06-Clean-Craftsmanship.md) | *Clean Craftsmanship* (2021) | Disciplines, standards, ethics | Going deep on TDD and test doubles; setting a team charter |
| 07 | [07-Functional-Design.md](07-Functional-Design.md) | *Functional Design* (2023) | Functional programming and design | Writing immutable, concurrent-safe code; React reducers; event sourcing |
| 08 | [08-We-Programmers.md](08-We-Programmers.md) | *We, Programmers* (2024/25) | History of the profession and its future | Building perspective; senior/leadership conversations about AI and hype |

Related: [Eric Evans – Domain-Driven Design](../Eric%20Evans/00-README.md) (separate folder).

## Suggested reading order

1. **Clean Code** – the basics of readable code.
2. **Agile PPP** – SOLID and patterns, the toolkit for design.
3. **Clean Architecture** – the same ideas applied to whole systems.
4. **The Clean Coder** – how a professional behaves.
5. **Clean Agile** – how a team delivers.
6. **Clean Craftsmanship** – deeper TDD plus the Programmer's Oath.
7. **Functional Design** – the same principles in a functional style.
8. **We, Programmers** – the history, read whenever you like.
9. **Domain-Driven Design** (Eric Evans folder) – modeling the business at the heart of Clean Architecture.

## How the books fit together

```
            Clean Agile ─────────── how the TEAM delivers
                 │
          The Clean Coder ───────── how YOU behave as a professional
                 │
        Clean Architecture ──────── how the SYSTEM is structured
                 │
      Agile PPP (SOLID + Patterns) ─ how CLASSES & PACKAGES relate
                 │
             Clean Code ─────────── how each LINE / FUNCTION reads
```

## One-page cheat sheet

- **Boy Scout Rule:** leave the code cleaner than you found it.
- **SOLID:** one reason to change · extend without modifying · subtypes are substitutable · small interfaces · depend on abstractions.
- **Dependency Rule:** source-code dependencies point inward, toward the business rules. The database, the web and frameworks are details.
- **TDD:** Red → Green → Refactor; the three laws.
- **Professionalism:** say no honestly, say "I will … by …", give estimates as ranges (PERT), keep your disciplines under pressure.
- **Agile:** small iterations produce data; Good/Fast/Cheap/Done, pick three; velocity is a measurement, never a target; never cut quality.
- **The only way to go fast is to go well.**
