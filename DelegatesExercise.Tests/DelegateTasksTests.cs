using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

// Tests are run against both the student implementation and the reference solution.
// The reference solution tests should always pass; the student tests guide your work.

namespace DelegatesExercise.Tests;

public class StudentImplementationTests
{
    // ----------------------------
    // 1) Custom delegate
    // ----------------------------

    // These are "real" methods we can bind to the student's delegate type via CreateDelegate.
    private static int Add(int a, int b) => a + b;
    private static int Multiply(int a, int b) => a * b;
    private static int Subtract(int a, int b) => a - b;

    private static Assembly StudentAssembly => typeof(DelegatesExercise.DelegateTasks).Assembly;

    private static Type GetStudentIntOperationTypeOrFail()
    {
        // Students must create: namespace DelegatesExercise; public delegate int IntOperation(int x, int y);
        var t = StudentAssembly.GetType("DelegatesExercise.IntOperation", throwOnError: false);
        Assert.True(t is not null, "Missing delegate type: DelegatesExercise.IntOperation. Students must define it.");
        Assert.True(typeof(MulticastDelegate).IsAssignableFrom(t!), "DelegatesExercise.IntOperation exists but is not a delegate type.");
        return t!;
    }

    private static Delegate CreateStudentIntOperationDelegate(Type intOperationType, string localMethodName)
    {
        var mi = typeof(StudentImplementationTests).GetMethod(localMethodName, BindingFlags.NonPublic | BindingFlags.Static);
        Assert.True(mi is not null, $"Internal test method '{localMethodName}' was not found.");

        // Create a delegate instance of the student's delegate type bound to our static method.
        return Delegate.CreateDelegate(intOperationType, mi!);
    }

    private static MethodInfo GetStudentMethodOrFail(string name, Type[] expectedParameterTypes)
    {
        var candidates = typeof(DelegatesExercise.DelegateTasks)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == name)
            .ToList();

        Assert.True(candidates.Count > 0, $"Missing method: DelegatesExercise.DelegateTasks.{name} (public static).");

        // Find exact match by parameter types (important, because students might have overloads).
        var match = candidates.FirstOrDefault(m =>
        {
            var ps = m.GetParameters();
            if (ps.Length != expectedParameterTypes.Length) return false;
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].ParameterType != expectedParameterTypes[i]) return false;
            }
            return true;
        });

        Assert.True(match is not null,
            $"Method signature mismatch for {name}. Expected: {name}({string.Join(", ", expectedParameterTypes.Select(t => t.Name))}).");

        return match!;
    }

    [Fact]
    public void Apply_UsesDelegateCallSyntax()
    {
        var intOpType = GetStudentIntOperationTypeOrFail();
        var addDel = CreateStudentIntOperationDelegate(intOpType, nameof(Add));

        // Expect: public static int Apply(IntOperation op, int x, int y)
        var apply = GetStudentMethodOrFail(
            name: "Apply",
            expectedParameterTypes: new[] { intOpType, typeof(int), typeof(int) }
        );

        var result = apply.Invoke(null, new object[] { addDel, 3, 4 });
        Assert.IsType<int>(result);
        Assert.Equal(7, (int)result!);
    }

    [Fact]
    public void ApplyWithInvoke_UsesInvoke()
    {
        var intOpType = GetStudentIntOperationTypeOrFail();
        var mulDel = CreateStudentIntOperationDelegate(intOpType, nameof(Multiply));

        // Expect: public static int ApplyWithInvoke(IntOperation op, int x, int y)
        var applyWithInvoke = GetStudentMethodOrFail(
            name: "ApplyWithInvoke",
            expectedParameterTypes: new[] { intOpType, typeof(int), typeof(int) }
        );

        var result = applyWithInvoke.Invoke(null, new object[] { mulDel, 3, 4 });
        Assert.IsType<int>(result);
        Assert.Equal(12, (int)result!);
    }

    [Fact]
    public void CalculateAndLog_LogsAndReturns()
    {
        var intOpType = GetStudentIntOperationTypeOrFail();
        var subDel = CreateStudentIntOperationDelegate(intOpType, nameof(Subtract));

        var messages = new List<string>();
        Action<string> logger = msg => messages.Add(msg);

        // Expect: public static int CalculateAndLog(IntOperation operation, int x, int y, Action<string> logger)
        var calculateAndLog = GetStudentMethodOrFail(
            name: "CalculateAndLog",
            expectedParameterTypes: new[] { intOpType, typeof(int), typeof(int), typeof(Action<string>) }
        );

        var result = calculateAndLog.Invoke(null, new object[] { subDel, 10, 3, logger });
        Assert.IsType<int>(result);
        Assert.Equal(7, (int)result!);

        Assert.Single(messages);
        Assert.Equal("Result: 7", messages[0]);
    }

    // ----------------------------
    // 3) Predicate / Func / Action
    // ----------------------------

    [Fact]
    public void Filter_KeepsMatchingItems()
    {
        var data = new[] { 1, 2, 3, 4, 5, 6 };

        var evens = DelegatesExercise.DelegateTasks.Filter(data, x => x % 2 == 0);

        Assert.Equal(new List<int> { 2, 4, 6 }, evens);
    }

    [Fact]
    public void Map_TransformsItems()
    {
        var data = new[] { "a", "bb", "ccc" };

        var lengths = DelegatesExercise.DelegateTasks.Map(data, s => s.Length);

        Assert.Equal(new List<int> { 1, 2, 3 }, lengths);
    }

    [Fact]
    public void ForEach_ExecutesActionForEveryItem()
    {
        var data = new[] { 2, 3, 4 };
        int product = 1;

        DelegatesExercise.DelegateTasks.ForEach(data, x => product *= x);

        Assert.Equal(24, product);
    }

    // ----------------------------
    // 4) Returning delegates
    // ----------------------------

    [Fact]
    public void MakeMultiplier_ReturnsClosure()
    {
        var times3 = DelegatesExercise.DelegateTasks.MakeMultiplier(3);
        var times10 = DelegatesExercise.DelegateTasks.MakeMultiplier(10);

        Assert.Equal(30, times3(10));
        Assert.Equal(70, times10(7));
    }

    [Fact]
    public void Compose_ComposesTwoFunctions()
    {
        Func<int, int> plus1 = x => x + 1;
        Func<int, int> times2 = x => x * 2;

        var composed = DelegatesExercise.DelegateTasks.Compose(plus1, times2);

        // g(f(x)) => times2(plus1(5)) => 12
        Assert.Equal(12, composed(5));
    }

    // ----------------------------
    // 5) Strategy with Func
    // ----------------------------

    [Fact]
    public void CalculatePrice_AppliesDiscountStrategy()
    {
        decimal basePrice = 100m;

        // 10% discount
        decimal p1 = DelegatesExercise.DelegateTasks.CalculatePrice(basePrice, p => p * 0.9m);

        // conditional discount
        decimal p2 = DelegatesExercise.DelegateTasks.CalculatePrice(basePrice, p =>
        {
            if (p >= 100m) return p * 0.8m;
            return p;
        });

        Assert.Equal(90m, p1);
        Assert.Equal(80m, p2);
    }
}
