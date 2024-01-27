using Autodesk.Revit.UI;
using ricaun.Revit.UI.Tasks.Handlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks
{
    /// <summary>
    /// RevitTask service to manage and run code in Revit context.
    /// </summary>
    public class RevitTaskService : IDisposable
    {
        private readonly UIControlledApplication application;
        private List<ExternalEvent> ExternalEvents = new List<ExternalEvent>();
        private List<IExternalEventHandler> ExternalEventHandlers = new List<IExternalEventHandler>();
        private bool HasInitialized;

        /// <summary>
        /// RevitTaskService
        /// </summary>
        /// <param name="application"></param>
        public RevitTaskService(UIControlledApplication application)
        {
            this.application = application;
        }

        /// <summary>
        /// Initialize the service
        /// </summary>
        /// <remarks>Subscribe to the <see cref="Autodesk.Revit.UI.UIControlledApplication.Idling"/> event.</remarks>
        public void Initialize()
        {
            if (HasInitialized) return;
            application.Idling += Application_Idling;
            HasInitialized = true;
        }

        /// <summary>
        /// Run code in Revit context.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public Task<TResult> Run<TResult>(Func<UIApplication, TResult> function)
        {
            if (!HasInitialized)
                return Task.FromException<TResult>(new Exception($"{this.GetType().Name} is not initialized."));

            // Todo: run the function if already is in the Revit context.

            var asyncExternalEventHandler = new AsyncExternalEventHandler<TResult>(function);
            ExternalEventHandlers.Add(asyncExternalEventHandler);
            return asyncExternalEventHandler.AsyncResult();
        }

        private void Application_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {
            foreach (var asyncExternalEventHandler in ExternalEventHandlers)
            {
                if (ExternalEventHandlers.Remove(asyncExternalEventHandler))
                {
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
            application.Idling -= Application_Idling;

            ExternalEvents.Clear();
            ExternalEventHandlers.Clear();
            HasInitialized = false;
        }
    }

}
