using System;
using System.Threading;
using System.Threading.Tasks;

// 1. Create the source and retrieve the token
using var cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

// 2. Automatically request cancellation after 2 seconds
cts.CancelAfter(TimeSpan.FromSeconds(2));

try
{
    // 3. Pass the token to an async operation
    await Task.Delay(TimeSpan.FromSeconds(10), token);
    Console.WriteLine("Task completed.");
}
catch (OperationCanceledException)
{
    // 4. Handle the graceful termination
    Console.WriteLine("Task was cancelled.");
}
