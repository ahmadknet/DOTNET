# C# Extension Methods: Concept and Code Demo

## What Are Extension Methods?
**Extension methods** let you "add" new methods to existing types—such as `string`, `int`, interfaces like `IEnumerable<T>`, or third-party classes—without changing their source code, recompiling them, or creating new child classes.

Behind the scenes, they are **static methods**, but you call them just like normal **instance methods**. This makes your custom methods feel like built-in features of the original type.

### Why Should You Use Them?
* **Extend Built-in or Sealed Types:** You can add helpful features to types like `string` or `DateTime`, or to `sealed` classes that do not allow inheritance.
* **Upgrade Interfaces:** When you add an extension method to an interface (like `IEnumerable<T>`), every class that uses that interface (`List<T>`, arrays, etc.) gets the method automatically. This is how **LINQ** works.
* **Write Cleaner Code:** Instead of calling static helper classes like `StringHelper.ToTitleCase(text)`, you can write smooth, readable chains like `text.ToTitleCase()`.

---

## Key Rules to Remember
1. **Static Class Requirement:** You must write extension methods inside a **non-nested, non-generic `static` class**.
2. **Namespace Visibility:** An extension method only works if you import its namespace with a `using` statement.
3. **Public Access Only:** Because extension methods live outside the original class, they can only use `public` (or `internal` in the same project) properties and methods. They cannot access `private` fields.
4. **Original Methods Take Priority:** If a class already has a regular method with the exact same name and parameters, C# will always run the original method instead of your extension.
5. **Safe with `null`:** Calling an extension method on a `null` object does not immediately crash with a `NullReferenceException`. Instead, C# simply passes `null` into your method as a parameter.

---

## Two Ways to Write Extensions

### 1. Classic Syntax (`this` Keyword)
This works in all modern versions of C#. You make a `static` method and put the `this` keyword before the first parameter to show which type you are extending.

```csharp
public static class StringExtensions
{
    public static int WordCount(this string text) => ...
}
```

### 2. Modern Syntax (`extension` Blocks — C# 14+)
Introduced in C# 14 (and expanded in C# 15), `extension` blocks let you group multiple extension methods, properties, and operators together for one type without repeating `this`.

```csharp
public static class ModernStringExtensions
{
    extension(string text)
    {
        public bool IsBlank => string.IsNullOrWhiteSpace(text);
        public string Truncate(int maxLength) => ...
    }
}
```

---

## Complete Runnable Code Demo

Save this code in a C# console project (`Program.cs`) and run it with `dotnet run`.

```csharp
using System;
using System.Collections.Generic;
using MyApp.Extensions; // Required to make the extension methods visible

// ==========================================
// 1. USAGE DEMO
// ==========================================
string sentence = "C# extension methods make code very clean!";
Console.WriteLine($"Original:   {sentence}");
Console.WriteLine($"Word Count: {sentence.WordCount()}");
Console.WriteLine($"Truncated:  {sentence.Truncate(20)}");

// Calling an extension method on null (safe, does not crash)
string? emptyText = null;
Console.WriteLine($"Is Null or Blank: {emptyText.IsNullOrBlank()}");

// Generic interface extension working on both a List and an Array
List<int> numbers =;
string[] fruits = ["Apple", "Banana", "Cherry"];

Console.WriteLine($"Numbers: {numbers.ToDelimitedString(" -> ")}");
Console.WriteLine($"Fruits:  {fruits.ToDelimitedString(" | ")}");

// ==========================================
// 2. EXTENSION DEFINITIONS
// ==========================================
namespace MyApp.Extensions
{
    // Must be a non-nested, non-generic static class
    public static class CustomExtensions
    {
        // Classic syntax: extends 'string' using the 'this' keyword
        public static int WordCount(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            return text.Split([' ', '.', '!', '?'], StringSplitOptions.RemoveEmptyEntries).Length;
        }

        // Classic syntax with an extra parameter
        public static string Truncate(this string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;

            return text[..maxLength] + "...";
        }

        // Null-safe extension for a nullable string
        public static bool IsNullOrBlank(this string? text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        // Generic extension on IEnumerable<T> (works on all collections)
        public static string ToDelimitedString<T>(this IEnumerable<T> source, string delimiter = ", ")
        {
            return string.Join(delimiter, source);
        }
    }
}
```

### Expected Output
```text
Original:   C# extension methods make code very clean!
Word Count: 7
Truncated:  C# extension methods...
Is Null or Blank: True
Numbers: 10 -> 20 -> 30 -> 40 -> 50
Fruits:  Apple | Banana | Cherry
```
