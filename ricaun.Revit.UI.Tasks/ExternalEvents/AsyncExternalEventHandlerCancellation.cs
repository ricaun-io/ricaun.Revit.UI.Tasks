using Autodesk.Revit.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks.ExternalEvents
{
    /// <summary>
    /// AsyncExternalEventHandlerCancellation
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class AsyncExternalEventHandlerCancellation<TResult> : IExternalEventHandler, IDisposable
    {
        private readonly Func<UIApplication, TResult> function;
        private readonly TaskCompletionSourceCancellation<TResult> tcs;

        /// <summary>
        /// AsyncExternalEventHandlerCancellation
        /// </summary>
        /// <param name="function"></param>
        /// <param name="cancellationToken"></param>
        public AsyncExternalEventHandlerCancellation(Func<UIApplication, TResult> function, CancellationToken cancellationToken)
        {
            this.function = function;
            tcs = new TaskCompletionSourceCancellation<TResult>(cancellationToken);
        }

        /// <summary>
        /// AsyncResult
        /// </summary>
        /// <returns></returns>
        public Task<TResult> AsyncResult()
        {
            return tcs.Task;
        }

        /// <summary>
        /// This method is called to handle the external event.
        /// </summary>
        /// <param name="uiapp"></param>
        public void Execute(UIApplication uiapp)
        {
            try
            {
                var result = function.Invoke(uiapp);
                tcs.TrySetResult(result);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }

        /// <summary>
        /// String identification of the event handler.
        /// </summary>
        public string GetName()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Create <see cref="Autodesk.Revit.UI.ExternalEvent"/> using the <see cref="AsyncExternalEventHandler{TResult}"/>
        /// </summary>
        public ExternalEvent Create()
        {
            return ExternalEvent.Create(this);
        }

        /// <summary>
        /// Dispose the registration cancelation token.
        /// </summary>
        public void Dispose()
        {
            tcs.Dispose();
        }
    }

}
