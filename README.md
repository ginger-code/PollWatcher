# PollWatcher

PollWatcher provides a way to register handler delegates to poll-based monitoring solutions.
This enables event-driven development in scenarios where events are not provided and cannot easily be added.
Possible use-cases include: polling a remote file server for changes, monitoring responses from an API, creating a recurring database task, etc.
Built leveraging the System.Reactive namespace


Example (C# 9.0 syntax): 
```c#
internal static class Program
{
    private static void Main()
    {
        using var watcher      = new PollWatcher<DateTime>(GetDate, interval, PrintDate, PrintException);
        // using var asyncWatcher = new AsyncPollWatcher<DateTime>(GetDateAsync, interval, PrintDate, PrintException);
        Thread.Sleep(5000);
    }

    // Returns a value that changes over time but must be retrieved synchronously
    // In this case, it's the time
    private static DateTime GetDate() => DateTime.Now;
    
    // Returns a value that changes over time but must be retrieved asynchronously
    // In this case, it's the time
    private static async Task<DateTime> GetDateAsync() => await Task.Run(() => DateTime.Now);

    //Specifies polling should occur every .001ms (if possible)
    private static TimeSpan interval = TimeSpan.FromMilliseconds(.001);

    // "Does stuff" with the data retrieved from polling
    // In this case, we print it to the console
    private static void PrintDate(DateTime dateTime) => Console.WriteLine(dateTime.ToString("O"));

    // "Does stuff" with exceptions raised during polling
    // In this case, we print it to the console
    private static void PrintException(Exception exception) => Console.WriteLine($"Exception!!!\t{exception.Message}");
}
```