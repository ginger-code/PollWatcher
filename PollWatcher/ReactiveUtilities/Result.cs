using System;
using System.Globalization;

namespace PollWatcher.ReactiveUtilities
{
    /// <summary>
    /// Result&lt;T&gt; represents either a Success&lt;T&gt;(T Value), or Error&lt;T&gt;(Exception Exception)
    /// Results should not contain null
    /// </summary>
    /// <typeparam name="T">The type of Value, if this is a Success</typeparam>
    internal abstract record Result<T>
    {
        public static Result<T> Create(T value) => new Success<T>(value);

        public static Result<T> Fail(Exception value) => new Error<T>(value);

        public abstract TResult Switch<TResult>(Func<T, TResult> caseValue, Func<Exception, TResult> caseError);

        public abstract void Switch(Action<T> caseValue, Action<Exception> caseError);
    }

    /// <summary>
    /// Represents a successful operation that returns data
    /// </summary>
    /// <typeparam name="T">The type of Value</typeparam>
    internal sealed record Success<T>(T Value) : Result<T>
    {
        public override TResult Switch<TResult>(Func<T, TResult> caseValue, Func<Exception, TResult> caseError) => caseValue(Value);

        public override void Switch(Action<T> caseValue, Action<Exception> caseError) => caseValue(Value);
        public          T    Value { get; } = Value;

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "Success({0})", Value);
    }

    /// <summary>
    /// Represents a failure that throws an exception
    /// </summary>
    /// <typeparam name="T">The type of Value, were this operation successful</typeparam>
    internal sealed record Error<T>(Exception Exception) : Result<T>
    {
        public override TResult Switch<TResult>(Func<T, TResult> caseValue, Func<Exception, TResult> caseError) => caseError(Exception);

        public override void      Switch(Action<T> caseValue, Action<Exception> caseError) => caseError(Exception);
        public          Exception Exception { get; } = Exception ?? throw new ArgumentNullException();

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "Error({0})", Exception);
    }
}
