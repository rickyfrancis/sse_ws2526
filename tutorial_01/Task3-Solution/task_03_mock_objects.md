# Tutorial 01 — Mock Objects — Study Guide

---

## Starting from the Beginning: Why Isolation Is a Problem

As covered earlier, a **unit test** is supposed to test one single unit of code in complete isolation. In theory this sounds straightforward — but in practice, almost no real-world class or function exists in a vacuum. Code has **dependencies**.

Consider a class `OrderService` that processes a customer's order. To do its job, it might need to:

- Query a **database** to check stock levels
- Call an **email service** to send a confirmation
- Hit a **payment API** to charge a credit card
- Write to a **log file**

If you write a unit test for `OrderService`, you immediately have a problem. Running the test would trigger real database queries, send real emails, charge real money, and write real files. That is obviously unacceptable. But beyond the practical absurdity, it also violates the core principle of unit testing: you are no longer testing _one unit_ — you are testing `OrderService` plus the database plus the email service plus the payment API all at once. If the test fails, you have no idea which component is responsible.

This is the problem that **Mock Objects** solve.

---

## What Is a Mock Object?

A **mock object** is a simulated, controlled replacement for a real dependency. It is a fake version of a collaborator that your unit under test needs, created specifically for the purposes of testing. The mock implements the same interface or inherits from the same base class as the real dependency, so the code under test cannot tell the difference — but instead of doing real work, it behaves in whatever way the test instructs it to.

The broader category of these fakes is called **test doubles** (a term borrowed from film stunt doubles — a stand-in that looks like the real thing from the outside). Mocks are one specific type of test double, and it's worth understanding the distinctions between the different kinds.

---

## Types of Test Doubles

### Dummy

A **dummy** is the simplest kind. It's an object passed around to satisfy a parameter requirement but never actually used. For example, if a method signature requires a `Logger` object but that code path never calls the logger in your specific test, you might pass a dummy logger — just to make the code compile and run.

### Stub

A **stub** is a test double that provides **pre-programmed, hardcoded return values** for method calls. When the code under test calls `database.GetStock("item_42")`, the stub simply returns `10` — no database is consulted. Stubs are used to control the _indirect inputs_ to the unit being tested. You use them to set up specific scenarios: what does `OrderService` do when stock is 0? When the payment API returns an error? A stub lets you simulate these conditions reliably.

### Mock

A **mock** goes one step further than a stub. In addition to providing return values, a mock **records how it was interacted with** and allows you to **verify those interactions** after the test runs. You can assert things like: "Was `emailService.SendConfirmation()` called exactly once?" or "Was it called with the correct customer email address?" Mocks are about verifying _behavior_ — specifically, that the unit under test called its collaborators in the expected way.

### Fake

A **fake** is a lightweight, working implementation of a dependency that's designed for testing but isn't suitable for production. A classic example is an **in-memory database** — it actually stores and retrieves data like a real database, but keeps everything in a dictionary in memory rather than on disk. Fakes are more sophisticated than stubs but still deterministic and fast.

### Spy

A **spy** is similar to a mock but less strict. It wraps a real object and records calls made to it, letting you verify interactions after the fact — but it still delegates to the real implementation unless you explicitly override certain methods.

---

## Mock vs. Stub: The Key Distinction

This distinction often causes confusion, so it's worth being precise:

- A **stub** answers questions: _"What should I return when called?"_ You use it to set up indirect inputs.
- A **mock** verifies behavior: _"Was I called, and was I called correctly?"_ You use it to assert on indirect outputs.

In practice, many mocking frameworks (like Moq in C#, Mockito in Java) blend both capabilities into a single object, which is why "mock" is often used loosely to refer to any test double.

---

## When Should Mock Objects Be Used?

Mocks are appropriate in specific situations. Using them everywhere is an anti-pattern; the goal is always to use the simplest test double that gets the job done.

**Use mocks/test doubles when a dependency:**

**Is slow.** Database queries, file system access, and network calls can take seconds. A unit test suite with hundreds of real database calls would take minutes to run, destroying the fast feedback loop that makes unit tests valuable. A mock returns instantly.

**Has side effects.** If calling a dependency changes the state of the external world — sending an email, charging a credit card, posting a tweet, deleting a file — you cannot call it in a test. A mock lets you verify that the call _would have been made_ without actually making it.

**Is non-deterministic.** If a dependency returns different values each time (the current time, a random number, a live stock price), your tests become flaky and unreliable. A stub lets you control exactly what is returned so your test always runs under the same conditions.

**Doesn't exist yet.** If you're developing a component whose dependencies haven't been built yet, mocks let you test your component against a simulated version of the future dependency, allowing parallel development.

**Is difficult to set up in a specific state.** Simulating an error condition — like a database going offline or a network timeout — is extremely difficult with real infrastructure. A mock makes it trivial: simply tell the mock to throw an exception when called.

---

## A Concrete C# Example (Using Moq)

Imagine you have this interface and service:

```csharp
public interface IEmailService
{
    void SendConfirmation(string email, string orderId);
}

public class OrderService
{
    private readonly IEmailService _emailService;

    public OrderService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public void PlaceOrder(string customerEmail, string orderId)
    {
        // ... process order logic ...
        _emailService.SendConfirmation(customerEmail, orderId);
    }
}
```

A unit test using a mock (with the Moq framework in C#):

```csharp
[Test]
public void PlaceOrder_ShouldSendConfirmationEmail()
{
    // Arrange
    var mockEmailService = new Mock<IEmailService>();
    var orderService = new OrderService(mockEmailService.Object);

    // Act
    orderService.PlaceOrder("ricky@example.com", "order-99");

    // Assert — verify the mock was called correctly
    mockEmailService.Verify(
        e => e.SendConfirmation("ricky@example.com", "order-99"),
        Times.Once
    );
}
```

No real email is sent. The mock records that `SendConfirmation` was called, and the test verifies the exact arguments and call count. The test is fast, deterministic, and tests exactly one thing.

---

## The Role of Dependency Injection

For mocks to work, the code under test must **receive its dependencies from outside** rather than creating them internally. If `OrderService` did `new SmtpEmailService()` inside its constructor, there would be no way to substitute a mock. This is why mock objects go hand in hand with **Dependency Injection (DI)** — the practice of passing dependencies in through the constructor (or a property/method), so that in tests you can pass a mock instead of the real thing.

This is also why writing testable code tends to produce better-designed code: it forces you to define clear interfaces between components and avoid tight coupling.

---

## When NOT to Use Mocks

Mocks can be overused. If you mock everything, your tests verify that your code calls other code — but not that the system actually works. Some failure modes:

**Don't mock the unit under test itself.** That defeats the entire purpose.

**Don't mock simple value objects or data classes.** If a dependency is just a data container with no side effects, use the real thing.

**Don't mock just to avoid writing a slightly inconvenient setup.** If the real dependency is fast, has no side effects, and is easy to configure, use it. This is where integration tests are appropriate.

**Over-mocking leads to brittle tests** that are tightly coupled to implementation details. If your test breaks every time you rename a private method, you've mocked too deeply.

---

## Summary Table

| Test Double | What It Does                          | When to Use                                                  |
| ----------- | ------------------------------------- | ------------------------------------------------------------ |
| **Dummy**   | Placeholder, never used               | Satisfying required parameters                               |
| **Stub**    | Returns hardcoded values              | Controlling indirect inputs / simulating scenarios           |
| **Mock**    | Records calls + verifies interactions | Asserting indirect outputs / verifying behavior              |
| **Fake**    | Lightweight working implementation    | When a stub is too simple but a real dependency is too heavy |
| **Spy**     | Wraps real object, records calls      | When you want real behavior but also want to observe it      |

---

## Key Takeaways for Your Exam

- A **mock object** is a fake replacement for a real dependency used in unit testing to achieve isolation.
- The umbrella term is **test double**; mocks, stubs, dummies, fakes, and spies are all specific kinds.
- The key difference: a **stub** controls return values (indirect inputs); a **mock** also _verifies interactions_ (indirect outputs).
- Use mocks when dependencies are **slow, have side effects, are non-deterministic, or don't exist yet**.
- Mocks require **Dependency Injection** to work — dependencies must be passed in from outside so they can be swapped in tests.
- Don't over-mock — tests that verify only interactions without testing real behavior can give a false sense of security.
