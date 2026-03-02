using System;
using System.Collections.Generic;

namespace DelegatesExercise;

/// <summary>
/// Delegate type representing an operation on two integers.
/// TODO: Define the delegate type as described in the README.
/// </summary>
/// <remarks>
/// Signature required by the tests:
///     int (int x, int y)
/// Name: IntOperation
/// </remarks>
public delegate int IntOperation(int x, int y);

public static class DelegateTasks
{
    // ----------------------------------------------------------------------
    // 1) Custom delegate: create and invoke
    // ----------------------------------------------------------------------

    /// <summary>
    /// Uses the provided IntOperation to compute a result for (x, y).
    /// This should invoke the delegate using the short syntax: op(x, y).
    /// </summary>
    public static int Apply(IntOperation op, int x, int y)
    {
        if (op is null) throw new ArgumentNullException(nameof(op));
        return op(x, y);
    }

    /// <summary>
    /// Same as Apply, but explicitly uses op.Invoke(x, y).
    /// </summary>
    public static int ApplyWithInvoke(IntOperation op, int x, int y)
    {
        if (op is null) throw new ArgumentNullException(nameof(op));
        return op.Invoke(x, y);
    }

    // ----------------------------------------------------------------------
    // 2) Higher-order function: delegate as parameter + logging Action
    // ----------------------------------------------------------------------

    /// <summary>
    /// Calls the operation and logs "Result: {value}" using the provided logger.
    /// Returns the computed result.
    /// </summary>
    public static int CalculateAndLog(IntOperation operation, int x, int y, Action<string> logger)
    {
        if (operation is null) throw new ArgumentNullException(nameof(operation));
        if (logger is null) throw new ArgumentNullException(nameof(logger));

        int result = operation(x, y);
        logger($"Result: {result}");
        return result;
    }

    // ----------------------------------------------------------------------
    // 3) Predefined delegates: Predicate, Func, Action
    // ----------------------------------------------------------------------

    /// <summary>
    /// Filters items using the provided predicate (keep items where predicate(item) is true).
    /// Returns a new List containing the kept items.
    /// </summary>
    public static List<T> Filter<T>(IEnumerable<T> items, Predicate<T> predicate)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        var result = new List<T>();
        foreach (var item in items)
        {
            if (predicate(item)) result.Add(item);
        }
        return result;
    }

    /// <summary>
    /// Maps each item to a result using selector and returns a new List of results.
    /// </summary>
    public static List<TResult> Map<T, TResult>(IEnumerable<T> items, Func<T, TResult> selector)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var result = new List<TResult>();
        foreach (var item in items)
        {
            result.Add(selector(item));
        }
        return result;
    }

    /// <summary>
    /// Executes the given action for each item in the sequence.
    /// </summary>
    public static void ForEach<T>(IEnumerable<T> items, Action<T> action)
    {
        if (items is null) throw new ArgumentNullException(nameof(items));
        if (action is null) throw new ArgumentNullException(nameof(action));

        foreach (var item in items)
        {
            action(item);
        }
    }

    // ----------------------------------------------------------------------
    // 4) Returning delegates: factories and composition
    // ----------------------------------------------------------------------

    /// <summary>
    /// Returns a function that multiplies its input by the given factor.
    /// Example: MakeMultiplier(3)(10) == 30.
    /// </summary>
    public static Func<int, int> MakeMultiplier(int factor)
    {
        return x => x * factor;
    }

    /// <summary>
    /// Returns a function that applies <paramref name="first"/> and then <paramref name="second"/>.
    /// Composition: Compose(f, g)(x) == g(f(x)).
    /// </summary>
    public static Func<T, T> Compose<T>(Func<T, T> first, Func<T, T> second)
    {
        if (first is null) throw new ArgumentNullException(nameof(first));
        if (second is null) throw new ArgumentNullException(nameof(second));
        return x => second(first(x));
    }

    // ----------------------------------------------------------------------
    // 5) Small "strategy" example with Func
    // ----------------------------------------------------------------------

    /// <summary>
    /// Calculates a final price by applying the given discount strategy.
    /// The strategy receives the basePrice and returns the discounted price.
    /// </summary>
    public static decimal CalculatePrice(decimal basePrice, Func<decimal, decimal> discountStrategy)
    {
        if (discountStrategy is null) throw new ArgumentNullException(nameof(discountStrategy));
        return discountStrategy(basePrice);
    }
}
