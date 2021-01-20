using System;
using System.Threading.Tasks;

namespace PollWatcher.Interfaces
{
    /// <summary>
    /// An asynchronous PollWatcher
    /// </summary>
    /// <typeparam name="T">Type of object returned by function contained in PollingTarget</typeparam>
    public interface IAsyncPollWatcher<T> : IDisposable
    {
        /// <summary>
        /// Asynchronous function used to poll for data
        /// </summary>
        Func<Task<T>> PollingTarget { get; }

        /// <summary>
        /// Interval at which to poll for data
        /// </summary>
        TimeSpan PollingInterval { get; }

        /// <summary>
        /// Action to perform on retrieved data
        /// </summary>
        Action<T> SuccessHandler { get; }

        /// <summary>
        /// Action to perform on exceptions raised during polling
        /// </summary>
        Action<Exception> FailureHandler { get; }
    }
}
