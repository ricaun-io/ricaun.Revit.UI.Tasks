using Autodesk.Revit.UI;
using System;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks
{
    public class RevitTask : IRevitTask
    {
        private readonly RevitTaskService revitTaskService;

        public RevitTask(RevitTaskService revitTaskService)
        {
            this.revitTaskService = revitTaskService;
        }
        public Task<TResult> Run<TResult>(Func<UIApplication, TResult> function)
        {
            return revitTaskService.Run(function);
        }
    }

}
