using System;
using System.Threading;
using System.Threading.Tasks;

namespace PollWatcher.Demo
{
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

        //Specifies polling should occur every 1ms
        private static TimeSpan interval = TimeSpan.FromMilliseconds(.001);

        // "Does stuff" with the data retrieved from polling
        // In this case, we print it to the console
        private static void PrintDate(DateTime dateTime) => Console.WriteLine(dateTime.ToString("O"));

        // "Does stuff" with exceptions raised during polling
        // In this case, we print it to the console
        private static void PrintException(Exception exception) => Console.WriteLine($"Exception!!!\t{exception.Message}");
    }
}
