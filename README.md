[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/YLZ5T7KK)
# Delegate Workshop — Student Exercise (C#)

This exercise practices **delegates** and **higher-order functions** in C#.

You will implement methods in `DelegatesExercise/DelegateTasks.cs` so that the unit tests pass.

## Learning goals

By completing this exercise, you should be able to:

- Define your **own delegate type**
- Create delegate **variables** and **invoke** them (`op(x, y)` and `op.Invoke(x, y)`)
- Write methods that **take delegates as parameters** and call them (higher-order functions)
- Use predefined delegate types:
  - `Action` / `Action<T...>`
  - `Func<...>`
  - `Predicate<T>`
- Use **lambda expressions** as delegate implementations

## Project structure

- `DelegatesExercise/`  
  Student project (you implement TODOs here)

- `DelegatesExercise.Solution/`  
  Reference solution (for teachers / self-check)

- `DelegatesExercise.Tests/`  
  xUnit unit tests.  
  The tests are run **twice**:
  - once against the student project (`DelegatesExercise`)
  - once against the solution project (`DelegatesExercise.Solution`)  
  This makes it easy to verify the tests themselves are correct.

## What to do

1. Open `DelegatesExercise/DelegateTasks.cs`
2. Implement every `TODO` (remove `throw new NotImplementedException()`).
3. Run the tests locally (example commands below).

## Suggested workflow (local)

Requirements: **.NET SDK 8+**

From the folder that contains the solution:

```bash
dotnet test
```

## Notes / hints

- Prefer writing small, readable methods.
- Don’t overthink generics: the test names describe what each method should do.
- Use `ArgumentNullException.ThrowIfNull(...)` where appropriate.

Good luck 🙂
