using System.Net.Http;

Console.WriteLine("Fetching data...");

// Start a task and do other work before awaiting it
Task<string> dataTask = FetchDataAsync("https://www.google.com");
Console.WriteLine("Doing other work while waiting...");

// Suspend execution until the task finishes
string result = await dataTask;
Console.WriteLine($"Downloaded {result.Length} characters.");

static async Task<string> FetchDataAsync(string url)
{
    using var client = new HttpClient();
    Console.WriteLine(client.GetStringAsync(url).ToString());
    return await client.GetStringAsync(url);
}
