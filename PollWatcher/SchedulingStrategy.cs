namespace PollWatcher
{
    /// <summary>
    /// Threading strategy for subscription
    /// </summary>
    public enum SchedulingStrategy
    {
        /// <summary>
        /// Use ThreadPool dispatching for concurrency without blocking current thread
        /// </summary>
        ThreadPool,
        /// <summary>
        /// Block on the current thread
        /// </summary>
        CurrentThread,
        /// <summary>
        /// Use platform defaults 
        /// </summary>
        PlatformDefault
        
    }
}
