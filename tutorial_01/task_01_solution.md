# Unit Testing & Test-Driven Development — Study Guide

---

## What Is a Test?

In software development, a **test** is a procedure that verifies whether a piece of code behaves as expected. You define some input, run the code, and check whether the output matches what you intended. Tests can be run manually (a developer clicks through a UI) or **automatically** (code that tests other code). Automated tests are far more valuable in practice because they can be run instantly, repeatedly, and without human error.

The core idea is simple: instead of only finding bugs after deployment, you encode your expectations as executable checks that run continuously throughout development.

---

## What Are Unit Tests?

A **unit test** targets the smallest possible piece of functionality in isolation — typically a single function, method, or class. The goal is to verify that one logical "unit" of code does exactly what it's supposed to do, independent of everything else.

A unit test typically follows the **AAA pattern**:

- **Arrange** — set up the inputs and preconditions
- **Act** — call the function or method being tested
- **Assert** — check that the result matches your expectation

For example, if you have a function `add(a, b)`, a unit test would call `add(2, 3)` and assert that the result equals `5`. It doesn't test the database, the network, or the UI — just that one function.

To keep units truly isolated, **test doubles** (mocks, stubs, fakes) are used to replace dependencies like databases or external APIs with controlled stand-ins.

---

## Unit vs. Integration vs. System Tests

These three categories form what's often called the **testing pyramid**, where unit tests are the wide base (many, fast, cheap) and system tests are the narrow top (few, slow, expensive).

**Unit Tests** verify a single unit of code in complete isolation. They are extremely fast (milliseconds each), very precise about _where_ a bug is, and easy to write and maintain. However, they tell you nothing about whether different units work correctly _together_.

**Integration Tests** verify that two or more components interact correctly. For example, does your data-access layer correctly talk to the real database? Does your service class correctly call the API client? Integration tests are slower because they involve real infrastructure (a database, a file system, a network call). They catch a whole class of bugs that unit tests miss — mismatched interfaces, incorrect SQL queries, wrong serialization formats — but when they fail, it can be harder to pinpoint the exact cause.

**System Tests** (also called end-to-end tests) verify the entire application as a whole, from the user's perspective. A system test might launch the full application, simulate a user logging in, submitting a form, and verify that the correct data appears on screen. These tests are the most realistic but also the slowest, the most brittle (they break when UI changes), and the hardest to debug. They're valuable for validating critical user journeys but are expensive to maintain.

The key distinction in one sentence: unit tests verify _logic_, integration tests verify _connections_, and system tests verify _behavior from the user's point of view_.

---

## Advantages and Disadvantages of Unit Tests

### Advantages

**Early bug detection.** Bugs are caught at the moment of writing, when the context is fresh in your mind and fixing is cheap. A bug caught in testing costs far less than one caught in production.

**Regression safety.** Once you have a suite of tests, you can refactor or add features with confidence. If you break something, a test fails immediately and tells you exactly what broke. Without tests, refactoring is always a gamble.

**Living documentation.** Unit tests describe _how_ a piece of code is intended to be used and what it's supposed to return. A new developer reading your tests gains instant insight into the expected behavior, which is always up to date (unlike comments or wikis).

**Forces better design.** Code that is hard to unit test is almost always badly structured — tightly coupled, doing too many things, hard to isolate. Writing tests pressures you to write smaller, focused, decoupled functions, which improves the overall architecture.

**Faster debugging.** When a test fails, you know exactly which unit is broken and under what inputs. Compare that to manually clicking through a UI hoping to reproduce a bug.

### Disadvantages

**Time investment.** Writing tests takes time — sometimes as much time as writing the feature itself. This can feel like overhead, especially under deadline pressure, and it requires ongoing maintenance as code evolves.

**False sense of security.** Passing unit tests don't guarantee the system works. Units can each be correct in isolation but fail to work together. A 100% unit test pass rate with no integration tests can be misleading.

**Tests can become brittle.** Poorly written tests are tightly coupled to implementation details rather than behavior. A minor refactor that doesn't change behavior can break dozens of tests, creating a maintenance burden and eroding trust in the suite.

**They don't catch everything.** Unit tests are poor at finding performance issues, concurrency bugs, UI problems, or environment-specific failures. They are one tool among many.

**Initial learning curve.** Writing good tests — knowing what to test, how to isolate dependencies, how to avoid testing the wrong things — is a skill that takes time to develop.

---

## Test-Driven Development (TDD)

TDD is a **development methodology** that flips the conventional order: instead of writing code first and then testing it, you write the test first, watch it fail, then write just enough code to make it pass.

The philosophy is that tests aren't just a quality-assurance step — they are a _design tool_. By writing the test before the code, you're forced to think clearly about the API, the inputs, the outputs, and the edge cases before committing to an implementation.

---

## The TDD Lifecycle (Red–Green–Refactor)

TDD operates in a tight loop of three phases, repeated continuously:

### 1. Red — Write a Failing Test

Before writing any production code, you write a test for the next small piece of functionality you want to implement. At this point the test **must fail** (shown as red in most testing frameworks), because the code it's testing doesn't exist yet. If the test passes immediately, either you wrote the wrong test or that functionality already exists.

This phase forces you to think about _what_ you want the code to do before thinking about _how_ to do it. You're defining the contract first.

### 2. Green — Write the Minimum Code to Pass

Now you write the **simplest possible code** that makes the failing test pass. The emphasis on "simplest possible" is deliberate and important — you don't write extra logic, you don't anticipate future requirements, you don't generalize. You do the bare minimum. The goal here is just to get to green (passing).

This discipline prevents over-engineering and keeps the codebase lean.

### 3. Refactor — Clean Up Without Breaking Tests

With a passing test as your safety net, you now **improve the code** — eliminate duplication, improve naming, simplify logic, improve structure — without changing its behavior. After every refactoring step, you run the tests again to confirm you haven't broken anything. The tests give you the freedom to restructure boldly.

Then you start the loop again with the next test.

---

### Why This Cycle Matters

The Red–Green–Refactor cycle keeps development in very short, controlled iterations. At any moment you have a clear goal (make this one test pass), a safety net (all previously passing tests), and a built-in quality step (refactor). Over time, the test suite grows organically alongside the code, and the design continuously improves without accumulating technical debt.

TDD also has a psychological benefit: you always know when you're "done" with a unit of work — when the test goes green. This replaces vague uncertainty with clear, objective feedback.

---

## Summary Table

|                      | Unit Test             | Integration Test           | System Test                |
| -------------------- | --------------------- | -------------------------- | -------------------------- |
| **Scope**            | Single function/class | Multiple components        | Full application           |
| **Speed**            | Very fast             | Moderate                   | Slow                       |
| **Isolation**        | Complete (mocks used) | Partial                    | None                       |
| **What it catches**  | Logic errors          | Interface/interaction bugs | End-to-end behavior issues |
| **Maintenance cost** | Low                   | Medium                     | High                       |

---

## Key Takeaways for Your Exam

- A **unit test** tests one thing in isolation using the AAA pattern.
- **Integration tests** check that components work together; **system tests** check the full application end-to-end.
- Unit tests offer early bug detection, regression safety, and better design — but cost time, can give false security, and don't test interactions.
- **TDD** means: write a failing test first, then write minimal code to pass it, then refactor — repeat.
- The TDD cycle is **Red → Green → Refactor**, and it serves as both a testing and a _design_ discipline.
