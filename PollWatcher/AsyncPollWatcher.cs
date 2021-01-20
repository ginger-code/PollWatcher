using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PollWatcher.Interfaces;
using PollWatcher.ReactiveUtilities;

namespace PollWatcher
{
    /// <summary>
    /// An asynchronous PollWatcher
    /// </summary>
    /// <typeparam name="T">The type of data returned by the polling method</typeparam>
    public class AsyncPollWatcher<T> : IAsyncPollWatcher<T>
    {
        public Func<Task<T>>     PollingTarget   { get; }
        public TimeSpan          PollingInterval { get; }
        public Action<T>         SuccessHandler  { get; }
        public Action<Exception> FailureHandler  { get; }


        /// <summary>
        /// Creates a new AsyncPollWatcher for async polling methods
        /// </summary>
        /// <param name="pollingTarget">Async function to poll for data</param>
        /// <param name="pollingInterval">Interval at which to poll for data</param>
        /// <param name="successHandler">Action to perform on retrieved data</param>
        /// <param name="failureHandler">Action to perform on exceptions raised during polling</param>
        /// <param name="schedulingStrategy">[default = SchedulingStrategy.ThreadPool] Thread scheduling strategy to utilize.</param>
        public AsyncPollWatcher(Func<Task<T>>       pollingTarget,
                                TimeSpan            pollingInterval,
                                Action<T>           successHandler,
                                Action<Exception>   failureHandler,
                                SchedulingStrategy? schedulingStrategy = null)
        {
            PollingInterval   = pollingInterval;
            SuccessHandler    = successHandler;
            FailureHandler    = failureHandler;
            PollingTarget     = pollingTarget;
            Scheduler         = schedulingStrategy.CreateScheduler();
            SourceFactory     = () => Observable.FromAsync(PollingTarget);
            PollingObservable = SourceFactory.Poll(PollingInterval, Scheduler);
            PollSubscription  = PollingObservable.Monitor(SuccessHandler, FailureHandler);
        }

        private IScheduler             Scheduler         { get; }
        private IDisposable            PollSubscription  { get; }
        private Func<IObservable<T>>   SourceFactory     { get; }
        private IObservable<Result<T>> PollingObservable { get; }


        public void Dispose()
        {
            PollSubscription.Dispose();
        }
    }
}
