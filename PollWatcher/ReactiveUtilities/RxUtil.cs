using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace PollWatcher.ReactiveUtilities
{
    /// <summary>
    /// Utility functions for observable polling
    /// Based on https://github.com/LeeCampbell/RxCookbook/blob/master/Repository/Polling.md
    /// </summary>
    internal static class RxUtil
    {
        /// <summary>
        /// Periodically repeats the observable sequence exposing a responses or failures.
        /// </summary>
        /// <typeparam name="T">The type of the sequence response values.</typeparam>
        /// <param name="sourceFactory">Function to create observables to be scheduled and resubscribed</param>
        /// <param name="period">The period of time to wait before subscribing to the <paramref name="sourceFactory"/> sequence. Subsequent subscriptions will occur this period after the previous sequence completes.</param>
        /// <param name="scheduler">The <see cref="IScheduler"/> to use to schedule the polling.</param>
        /// <returns>Returns an infinite observable sequence of values or errors.</returns>
        internal static IObservable<Result<T>> Poll<T>(this Func<IObservable<T>> sourceFactory, TimeSpan period, IScheduler scheduler) =>
            Observable.Timer(period, scheduler)                                                 //Fire the function every <period> 
                      .SelectMany(_ => sourceFactory())                                         //Flatten the response sequence.
                      .Select(Result<T>.Create)                                                 //Project successful values to the Result<T> return type.
                      .Catch<Result<T>, Exception>(ex => Observable.Return(Result<T>.Fail(ex))) //Project exceptions to the Result<T> return type
                      .Repeat();                                                                //Call function again for another value after the timer is triggered

        /// <summary>
        /// Used to subscribe to Poll&lt;T&gt;. Creates a subscription to an observable containing results with handlers for success and error cases 
        /// </summary>
        /// <param name="source">The source observable sequence containing Results</param>
        /// <param name="onSuccess">Handler for successes</param>
        /// <param name="onFailure">Handler for failures</param>
        /// <typeparam name="T">Return type of monitored observable</typeparam>
        /// <returns>A subscription to the observable sequence</returns>
        internal static IDisposable Monitor<T>(this IObservable<Result<T>> source, Action<T> onSuccess, Action<Exception> onFailure)
            => source.Subscribe(result => result.Switch(onSuccess, onFailure), onFailure); //Subscribe the success and failure handlers to the source observable


        /// <summary>
        /// Resolves a SchedulingStrategy enum into an Rx IScheduler
        /// </summary>
        internal static IScheduler CreateScheduler(this SchedulingStrategy? schedulingStrategy) => schedulingStrategy switch
        {
            SchedulingStrategy.CurrentThread => Scheduler.CurrentThread,
            SchedulingStrategy.PlatformDefault => Scheduler.Default,
            _ => ThreadPoolScheduler.Instance,
        };
    }
}
