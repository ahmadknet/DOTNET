using Microsoft.Extensions.DependencyInjection;

// 1. Setup the DI Container
var services = new ServiceCollection();

// Register dependencies: Map the interface to a concrete class.
// Try swapping EmailSender with SmsSender below without touching OrderService!
services.AddTransient<IMessageSender, EmailSender>();
services.AddTransient<OrderService>();

// Build the service provider
using var serviceProvider = services.BuildServiceProvider();

// 2. Resolve and run the main service
var orderService = serviceProvider.GetRequiredService<OrderService>();
orderService.PlaceOrder("Laptop");


// --- Abstractions & Implementations ---

// The contract (Interface)
public interface IMessageSender
{
    void Send(string message);
}

// Implementation A
public class EmailSender : IMessageSender
{
    public void Send(string message) => Console.WriteLine($"[Email]: {message}");
}

// Implementation B
public class SmsSender : IMessageSender
{
    public void Send(string message) => Console.WriteLine($"[SMS]: {message}");
}

// The Consumer (Receives its dependency via constructor injection)
public class OrderService
{
    private readonly IMessageSender _messageSender;

    public OrderService(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    public void PlaceOrder(string item)
    {
        Console.WriteLine($"Processing order for: {item}");
        _messageSender.Send($"Your order for {item} has been confirmed!");
    }
}
