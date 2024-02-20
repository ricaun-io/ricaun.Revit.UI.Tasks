using Autodesk.Revit.UI;
using ricaun.Revit.UI.Tasks.Extensions;
using ricaun.Revit.UI.Tasks.ExternalEvents;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks
{
    /// <summary>
    /// RevitTask service to manage and run code in Revit context.
    /// </summary>
    public class RevitTaskService : IDisposable, IRevitTask
    {
        private UIApplication uiapp;
        private List<ExternalEvent> ExternalEvents = new List<ExternalEvent>();
        private List<IExternalEventHandler> ExternalEventHandlers = new List<IExternalEventHandler>();
        private bool HasInitialized;

        /// <summary>
        /// RevitTaskService
        /// </summary>
        /// <param name="application"></param>
        public RevitTaskService(UIControlledApplication application) : this(application.GetUIApplication())
        {
        }

        /// <summary>
        /// RevitTaskService
        /// </summary>
        /// <param name="uiapp"></param>
        public RevitTaskService(UIApplication uiapp)
        {
            this.uiapp = uiapp;
        }

        /// <summary>
        /// Initialize the service
        /// </summary>
        /// <remarks>Subscribe to the <see cref="Autodesk.Revit.UI.UIControlledApplication.Idling"/> event.</remarks>
        public void Initialize()
        {
            if (HasInitialized) return;
            uiapp.Idling += Application_Idling;
            HasInitialized = true;
        }

        /// <summary>
        /// Run code in Revit context.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResult> Run<TResult>(Func<UIApplication, TResult> function, CancellationToken cancellationToken)
        {
            if (!HasInitialized)
                return Task.FromException<TResult>(new Exception($"{this.GetType().Name} is not initialized."));

            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<TResult>(cancellationToken);

            if (uiapp.InContext())
            {
                try
                {
                    var result = function.Invoke(uiapp);
                    return Task.FromResult(result);
                }
                catch (Exception ex)
                {
                    return Task.FromException<TResult>(ex);
                }
            }

            var asyncExternalEventHandler = new AsyncExternalEventHandlerCancellation<TResult>(function, cancellationToken);
            ExternalEventHandlers.Add(asyncExternalEventHandler);
            return asyncExternalEventHandler.AsyncResult();
        }

        private void Application_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            uiapp = sender as UIApplication;

            foreach (var asyncExternalEventHandler in ExternalEventHandlers)
            {
                if (ExternalEventHandlers.Remove(asyncExternalEventHandler))
                {
                    if (asyncExternalEventHandler is IDisposable disposable)
                        disposable.Dispose();

                    var externalEvent = ExternalEvent.Create(asyncExternalEventHandler);
                    externalEvent.Raise();
                    ExternalEvents.Add(externalEvent);
                    break;
                }
            }

            foreach (var externalEvent in ExternalEvents)
            {
                if (externalEvent.IsPending)
                    continue;

                if (ExternalEvents.Remove(externalEvent))
                {
                    externalEvent.Dispose();
                    break;
                }
            }
        }

        /// <summary>
        /// Dispose the service
        /// </summary>
        /// <remarks>Unsubscribe to the <see cref="Autodesk.Revit.UI.UIControlledApplication.Idling"/> event.</remarks>
        public void Dispose()
        {
            if (!HasInitialized) return;
            uiapp.Idling -= Application_Idling;

            ExternalEvents.Clear();
            ExternalEventHandlers.Clear();
            HasInitialized = false;
        }
    }

}
