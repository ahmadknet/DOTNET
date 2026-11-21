using System;

// 1. Declare the delegate type (defines the required method signature)
public delegate void Notify(string message);

class Program
{
    // 2. Define a method with a matching signature
    static void ShowAlert(string text)
    {
        Console.WriteLine($"Alert: {text}");
    }

    static void Main()
    {
        // 3. Instantiate and invoke the delegate
        Notify handler = ShowAlert;
        handler("System update complete.");
    }
}
