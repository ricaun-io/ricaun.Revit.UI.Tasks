using Autodesk.Revit.UI;
using System;
using System.Threading;
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
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResult> Run<TResult>(Func<UIApplication, TResult> function, CancellationToken cancellationToken);
    }
}
