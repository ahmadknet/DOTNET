# .NET Object-Oriented Programming (OOP) & Resource Management Guide

This comprehensive guide covers the foundational pillars of Object-Oriented Programming in C#/.NET, advanced runtime concepts, SOLID principles, `IDisposable` resource cleanup, and a complete runnable single-file example.

---

## 1. C# Abstraction

**Abstraction in C#** is a fundamental OOP concept focused on **hiding internal implementation details and exposing only the essential features** of an object. It models *what* an object does rather than *how* it does it, reducing code complexity and decoupling systems.

C# achieves abstraction using two main mechanisms: **Abstract Classes** and **Interfaces**.

### Abstract Classes
An abstract class is a restricted class declared with the `abstract` modifier. It acts as an incomplete blueprint intended only to serve as a base class for other classes.

* **No Instantiation:** You cannot create an object directly using `new` on an abstract class.
* **Mixed Members:** It can contain **abstract members** (methods or properties without a body) as well as **concrete members** (methods with full implementation).
* **Enforced Contracts:** Any non-abstract class inheriting from an abstract class **must** provide a concrete implementation for all inherited abstract members using the `override` keyword.

```csharp
using System;

public abstract class Vehicle
{
    public abstract void StartEngine();

    public void Honk()
    {
        Console.WriteLine("Beep beep!");
    }
}

public class Car : Vehicle
{
    public override void StartEngine()
    {
        Console.WriteLine("Car engine started: Vroom!");
    }
}
```

### Interfaces
An interface defines a **pure structural contract**. It specifies what methods, properties, events, or indexers a class must provide without dictating how they operate.

* **Multiple Inheritance:** A C# class can inherit from only one class, but it can implement **multiple interfaces**.
* **No State:** Traditional interfaces do not contain instance fields (variables).
* **Default Implementation:** Modern C# versions allow interfaces to supply optional default implementations for methods if needed.

```csharp
public interface IPlayable
{
    void Play();
}

public class AudioPlayer : IPlayable
{
    public void Play()
    {
        Console.WriteLine("Playing music stream...");
    }
}
```

### Abstract Classes vs. Interfaces

| Feature | Abstract Class | Interface |
| :--- | :--- | :--- |
| **Instantiation** | Cannot be instantiated. | Cannot be instantiated. |
| **Inheritance Limit** | A class can inherit from only **one** class. | A class can implement **multiple** interfaces. |
| **Method Types** | Can have both abstract and fully functional methods. | Historically only signatures; modern C# allows optional default bodies. |
| **Fields & State** | Can contain instance fields (variables) to maintain state. | Cannot contain instance fields. |
| **Access Modifiers** | Members can be `private`, `protected`, `public`, etc. | Members are implicitly `public` by default. |

### Abstraction vs. Encapsulation
* **Abstraction** hides complexity at the design level (exposing the "what").
* **Encapsulation** hides data at the implementation level (restricting access to object internals using modifiers like `private`).

---

## 2. The Four Pillars of OOP in .NET

### 1. Encapsulation (Data Hiding)
Encapsulation binds data (fields) and code (methods) together into a single unit while **restricting direct access** to internal state using Access Modifiers and Properties.

```csharp
public class BankAccount
{
    private decimal balance;

    public decimal Balance
    {
        get { return balance; }
    }

    public void Deposit(decimal amount)
    {
        if (amount > 0) balance += amount;
    }
}
```

### 2. Inheritance (Code Reuse)
Inheritance allows a derived (child) class to **absorb properties and behaviors** from a base (parent) class.

```csharp
public class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }
}

public class Developer : Employee
{
    public string ProgrammingLanguage { get; set; }
}
```

### 3. Polymorphism (Many Forms)
Polymorphism allows objects of different classes to be treated as objects of a common base class.
* **Compile-time Polymorphism:** Method Overloading (same method name, different parameters).
* **Runtime Polymorphism:** Method Overriding (using `virtual` in the base class and `override` in the child class).

```csharp
public class Animal
{
    public virtual void MakeSound() => Console.WriteLine("Animal sound");
}

public class Dog : Animal
{
    public override void MakeSound() => Console.WriteLine("Bark!");
}
```

### 4. Abstraction (Interface vs. Implementation)
Delivered via `abstract` classes and `interfaces` to define architectural contracts.

---

## 3. Core .NET OOP Types, Modifiers & Memory

### Class vs. Struct

| Feature | Class | Struct |
| :--- | :--- | :--- |
| **Type** | Reference Type | Value Type |
| **Memory Allocation** | Allocated on the **Heap** | Allocated on the **Stack** (or inline) |
| **Inheritance** | Supports full inheritance | Cannot inherit from other classes/structs |
| **Nullability** | Can be `null` by default | Cannot be `null` (unless made `Nullable<T>`) |
| **Best Used For** | Large, complex business entities | Small, lightweight data structures (e.g., Point, Color) |

### Advanced Access Modifiers
* **`internal`:** Accessible only within the **same assembly** (project).
* **`protected`:** Accessible within the containing class and by **derived class instances**.
* **`protected internal`:** Accessible within the **same assembly OR any derived class** (even in a different assembly).
* **`private protected`:** Accessible within the **containing class OR derived classes, but ONLY within the same assembly**.

### Memory Mechanics: Stack vs. Heap
* **Stack:** Stores primitive value types and object reference pointers. Memory is allocated and deallocated automatically when execution leaves a method scope.
* **Heap:** Stores actual reference-type object data. The **Garbage Collector (GC)** periodically scans the heap to delete unreferenced objects.

### Method Overloading vs. Method Overriding

| Feature | Method Overloading (Compile-Time) | Method Overriding (Runtime) |
| :--- | :--- | :--- |
| **Relationship** | Happens within the **same class** (usually). | Happens between **Base and Derived classes**. |
| **Signatures** | Must have **different parameters** (types or count). | Must have the **exact same signature** and return type. |
| **Keywords** | No special keywords required. | Base needs `virtual`/`abstract`; Child needs `override`. |
| **Execution** | Compiler chooses the method at compilation. | CLR chooses the method at runtime based on actual object type. |

---

## 4. SOLID Principles in .NET

* **S - Single Responsibility:** A class should have **only one reason to change** (one job).
* **O - Open/Closed:** Classes should be **open for extension, but closed for modification**.
* **L - Liskov Substitution:** Child classes must be able to **replace parent classes smoothly** without breaking application logic.
* **I - Interface Segregation:** Prefer **many small, specific interfaces** over one massive, bloated interface.
* **D - Dependency Inversion:** Depend on **abstractions (interfaces), not concrete classes** (the foundation of Dependency Injection).

---

## 5. Implementing `IDisposable` for Unmanaged Resources

The **`IDisposable`** interface provides a standard mechanism for releasing **unmanaged resources** (file handles, database connections, raw memory pointers, network sockets) immediately when an object is no longer needed.

### Basic Pattern (Wrapping Managed `IDisposable` Objects)

```csharp
using System;
using System.IO;

public class SimpleFileLogger : IDisposable
{
    private FileStream _fileStream;
    private bool _disposed = false;

    public SimpleFileLogger(string filePath)
    {
        _fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _fileStream?.Dispose();
            _disposed = true;
        }
    }
}
```

### Full Dispose Pattern (With Finalizer for Raw Unmanaged Resources)

```csharp
using System;
using System.Runtime.InteropServices;

public class UnmanagedBuffer : IDisposable
{
    private IntPtr _nativeMemory;
    private bool _disposed = false;

    public UnmanagedBuffer(int size)
    {
        _nativeMemory = Marshal.AllocHGlobal(size);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Dispose managed objects here
        }

        if (_nativeMemory != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(_nativeMemory);
            _nativeMemory = IntPtr.Zero;
        }

        _disposed = true;
    }

    ~UnmanagedBuffer()
    {
        Dispose(false);
    }
}
```

### Consuming `IDisposable` Safely
* **`using` Statement:** `using (var logger = new SimpleFileLogger("app.log")) { ... }`
* **`using` Declaration (C# 8+):** `using var logger = new SimpleFileLogger("app.log");`

### Synchronous vs. Asynchronous Cleanup

| Interface | Method | Best Used For |
| :--- | :--- | :--- |
| **`IDisposable`** | `void Dispose()` | Standard synchronous cleanup of files, memory pointers, and local handles. |
| **`IAsyncDisposable`** | `ValueTask DisposeAsync()` | Asynchronous cleanup where flushing buffers or closing network streams involves I/O latency (`await using`). |

---

## 6. Complete Single-File Runnable Example (`Program.cs`)

Below is a complete, runnable C# console program demonstrating Abstraction, all Four Pillars of OOP, SOLID principles, and both `IDisposable` patterns together.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNetOopMasterclass
{
    // =========================================================================
    // 1. ABSTRACTION & INTERFACE SEGREGATION (SOLID)
    // =========================================================================
    
    public interface ILogger
    {
        void Log(string message);
    }

    public abstract class SystemComponent
    {
        public string ComponentName { get; }

        protected SystemComponent(string name)
        {
            ComponentName = name;
        }

        public abstract void Execute();

        public void ReportStatus()
        {
            Console.WriteLine($"[Status] {ComponentName} is operational.");
        }
    }

    // =========================================================================
    // 2. ENCAPSULATION & IDISPOSABLE (Basic Managed Wrapper Pattern)
    // =========================================================================
    
    public class FileLogger : ILogger, IDisposable
    {
        private readonly FileStream _fileStream;
        private bool _disposed = false;

        public FileLogger(string filePath)
        {
            _fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
        }

        public void Log(string message)
        {
            if (_disposed) 
                throw new ObjectDisposedException(nameof(FileLogger));

            string formatted = $"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}";
            byte[] bytes = Encoding.UTF8.GetBytes(formatted);
            _fileStream.Write(bytes, 0, bytes.Length);
            _fileStream.Flush();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _fileStream?.Dispose();
                _disposed = true;
                Console.WriteLine("[Cleanup] FileLogger closed the underlying file stream.");
            }
        }
    }

    // =========================================================================
    // 3. FULL DISPOSE PATTERN (Unmanaged Memory + Finalizer Safety Net)
    // =========================================================================
    
    public class UnmanagedMemoryBuffer : IDisposable
    {
        private IntPtr _nativePointer;
        private bool _disposed = false;

        public int Size { get; }

        public UnmanagedMemoryBuffer(int sizeInBytes)
        {
            Size = sizeInBytes;
            _nativePointer = Marshal.AllocHGlobal(sizeInBytes);
            Console.WriteLine($"[Memory] Allocated {sizeInBytes} bytes of unmanaged memory.");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Clean up managed IDisposable objects here (if any)
            }

            if (_nativePointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_nativePointer);
                _nativePointer = IntPtr.Zero;
                Console.WriteLine("[Cleanup] Unmanaged native memory freed.");
            }

            _disposed = true;
        }

        ~UnmanagedMemoryBuffer()
        {
            Dispose(false);
        }
    }

    // =========================================================================
    // 4. INHERITANCE, POLYMORPHISM & DEPENDENCY INVERSION (SOLID)
    // =========================================================================
    
    public class DataProcessor : SystemComponent
    {
        private readonly ILogger _logger;

        public DataProcessor(string name, ILogger logger) : base(name)
        {
            _logger = logger;
        }

        public override void Execute()
        {
            _logger.Log($"Starting execution for {ComponentName}...");
            
            using var buffer = new UnmanagedMemoryBuffer(1024);
            
            Console.WriteLine($"[Work] {ComponentName} is processing data using {buffer.Size}-byte buffer.");
            _logger.Log($"Completed execution for {ComponentName}.");
        }

        public void Execute(string customTask)
        {
            _logger.Log($"Running custom task: {customTask}");
            Console.WriteLine($"[Work] {ComponentName} executed custom task: {customTask}");
        }
    }

    // =========================================================================
    // 5. ENTRY POINT & EXECUTION
    // =========================================================================
    
    public class Program
    {
        public static void Main()
        {
            string logPath = "system_audit.log";
            Console.WriteLine("=== Starting .NET OOP & Resource Management Demo ===\n");

            using (FileLogger logger = new FileLogger(logPath))
            {
                SystemComponent processor = new DataProcessor("CoreEngine", logger);
                
                processor.ReportStatus();
                processor.Execute();

                if (processor is DataProcessor concreteProcessor)
                {
                    concreteProcessor.Execute("Nightly Database Sync");
                }
            }

            Console.WriteLine($"\n=== Demo Complete. Check '{logPath}' for output. ===");
        }
    }
}
