using Autodesk.Revit.UI;

namespace ricaun.Revit.UI.Tasks.Extensions
{
    /// <summary>
    /// RevitInContextExtension
    /// </summary>
    public static class RevitInContextExtension
    {
        /// <summary>
        /// Check Revit Context.
        /// </summary>
        /// <remarks>This method tries to subscribe to the <see cref="Autodesk.Revit.UI.UIApplication.Idling"/> event to check if Revit is in context. Revit API only allows subscribing to a Revit event when Revit is currently within an API context.</remarks>
        public static bool InContext(this UIApplication application)
        {
            try
            {
                application.Idling += Application_Idling;
                application.Idling -= Application_Idling;
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Check Revit Context.
        /// </summary>
        /// <remarks>This method tries to subscribe to the <see cref="Autodesk.Revit.UI.UIApplication.Idling"/> event to check if Revit is in context. Revit API only allows subscribing to a Revit event when Revit is currently within an API context.</remarks>
        public static bool InContext(this UIControlledApplication application)
        {
            try
            {
                application.Idling += Application_Idling;
                application.Idling -= Application_Idling;
                return true;
            }
            catch { }
            return false;
        }

        private static void Application_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e) { }
    }
}
