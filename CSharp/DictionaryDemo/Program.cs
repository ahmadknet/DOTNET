using System;
using System.Collections.Generic;



// 1. Initialize a dictionary
Dictionary<string, int> ages = new Dictionary<string, int>
{
    { "Alice", 30 },
    { "Bob", 25 }
};

// 2. Add or update items
ages.Add("Charlie", 35);   // Throws ArgumentException if "Charlie" exists
ages["Alice"] = 31;         // Adds if missing, or overwrites if present

// 3. Safe lookup (avoids KeyNotFoundException)
if (ages.TryGetValue("Bob", out int bobAge))
{
    Console.WriteLine($"Bob is {bobAge}");
}

// 4. Iterate through pairs
foreach (KeyValuePair<string, int> entry in ages)
{
    Console.WriteLine($"{entry.Key}: {entry.Value}");
}

// 5. Remove an item
ages.Remove("Charlie");

