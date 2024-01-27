using Autodesk.Revit.UI;
using System;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks
{
    /// <summary>
    /// Interface to run code in Revit Context
    /// </summary>
    public interface IRevitTask
    {
        /// <summary>
        /// Run code in Revit context.
        /// </summary>
        public Task<TResult> Run<TResult>(Func<UIApplication, TResult> function);
    }
}
