using Autodesk.Revit.UI;
using System;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks.ExternalEvents
{
    /// <summary>
    /// AsyncExternalEventHandler
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class AsyncExternalEventHandler<TResult> : IExternalEventHandler
    {
        private readonly Func<UIApplication, TResult> function;
        private readonly TaskCompletionSource<TResult> tcs;
        /// <summary>
        /// AsyncExternalEventHandler
        /// </summary>
        /// <param name="function"></param>
        public AsyncExternalEventHandler(Func<UIApplication, TResult> function)
        {
            this.function = function;
            tcs = new TaskCompletionSource<TResult>();
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
    }

}
