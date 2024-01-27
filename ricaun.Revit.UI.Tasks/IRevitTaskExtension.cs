using Autodesk.Revit.UI;
using System;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks
{
    /// <summary>
    /// Extension for <see cref="IRevitTask"/>
    /// </summary>
    public static class IRevitTaskExtension
    {
        /// <summary>
        /// Run code in Revit context.
        /// </summary>
        public static Task<TResult> Run<TResult>(this IRevitTask revitTask, Func<TResult> function)
        {
            return revitTask.Run<TResult>(uiapp => function());
        }
        /// <summary>
        /// Run code in Revit context.
        /// </summary>
        public static Task Run(this IRevitTask revitTask, Action<UIApplication> action)
        {
            return revitTask.Run<object>(uiapp => { action(uiapp); return null; });
        }
        /// <summary>
        /// Run code in Revit context.
        /// </summary>
        public static Task Run(this IRevitTask revitTask, Action action)
        {
            return revitTask.Run(uiapp => action());
        }
    }
}
