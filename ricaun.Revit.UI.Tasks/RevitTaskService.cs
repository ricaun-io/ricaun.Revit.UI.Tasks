using Autodesk.Revit.UI;
using ricaun.Revit.UI.Tasks.Handlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks
{
    public class RevitTaskService : IDisposable
    {
        private readonly UIControlledApplication application;
        private List<ExternalEvent> ExternalEvents = new List<ExternalEvent>();
        private List<IExternalEventHandler> ExternalEventHandlers = new List<IExternalEventHandler>();
        private bool HasInitialized;

        public RevitTaskService(UIControlledApplication application)
        {
            this.application = application;
        }

        public void Initialize()
        {
            if (HasInitialized) return;
            application.Idling += Application_Idling;
            HasInitialized = true;
        }

        public Task<TResult> Run<TResult>(Func<UIApplication, TResult> function)
        {
            if (!HasInitialized)
                return Task.FromException<TResult>(new Exception($"{this.GetType().Name} is not initialized."));

            // Todo: run the function if alrady is in the revit context.

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
