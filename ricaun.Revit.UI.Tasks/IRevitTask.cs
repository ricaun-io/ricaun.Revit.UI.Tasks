using Autodesk.Revit.UI;
using System;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks
{
    public interface IRevitTask
    {
        public Task<TResult> Run<TResult>(Func<UIApplication, TResult> function);
    }
}
