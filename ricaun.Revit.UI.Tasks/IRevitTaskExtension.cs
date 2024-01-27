using Autodesk.Revit.UI;
using System;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks
{
    public static class IRevitTaskExtension
    {
        public static Task<TResult> Run<TResult>(this IRevitTask revitTask, Func<TResult> function)
        {
            return revitTask.Run<TResult>(uiapp => function());
        }
        public static Task Run(this IRevitTask revitTask, Action<UIApplication> action)
        {
            return revitTask.Run<object>(uiapp => { action(uiapp); return null; });
        }
        public static Task Run(this IRevitTask revitTask, Action action)
        {
            return revitTask.Run(uiapp => action());
        }
    }

}
