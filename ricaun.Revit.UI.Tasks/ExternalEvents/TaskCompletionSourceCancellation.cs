using System;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks.ExternalEvents
{
    /// <summary>
    /// <see cref="TaskCompletionSource{TResult}"/> with cancellation token.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    internal class TaskCompletionSourceCancellation<TResult> : TaskCompletionSource<TResult>, IDisposable
    {
        private readonly IDisposable registration;

        /// <summary>
        /// Creates a <see cref="TaskCompletionSource{TResult}"/> with cancellation token.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public TaskCompletionSourceCancellation(CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
                cancellationToken = CancellationToken.None;

            if (cancellationToken.IsCancellationRequested)
            {
                TrySetCanceled(cancellationToken);
                return;
            }
            registration = cancellationToken.Register(() => TrySetCanceled(cancellationToken));
        }

        /// <summary>
        /// Dispose the registration cancelation token.
        /// </summary>
        public void Dispose()
        {
            registration?.Dispose();
        }
    }

}