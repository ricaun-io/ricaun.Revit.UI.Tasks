using Autodesk.Revit.UI;
using System;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks
{
    /// <summary>
    /// RevitTask
    /// </summary>
    [Obsolete("Use RevitTaskService instead. RevitTaskService has the interface IRevitTask.")]
    public class RevitTask : IRevitTask
    {
        private readonly RevitTaskService revitTaskService;

        /// <summary>
        /// RevitTask
        /// </summary>
        /// <param name="revitTaskService"></param>
        public RevitTask(RevitTaskService revitTaskService)
        {
            this.revitTaskService = revitTaskService;
        }
        /// <summary>
        /// Run code in Revit context.
        /// </summary>
        public Task<TResult> Run<TResult>(Func<UIApplication, TResult> function)
        {
            return revitTaskService.Run(function);
        }
    }

}
