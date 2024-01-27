using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            var revitTask = App.RevitTask;

            var task = Task.Run(async () =>
            {
                await Task.Delay(1000);
                await revitTask.Run(() => { System.Console.WriteLine("1"); });
                await revitTask.Run((uiapp) => { System.Console.WriteLine("2"); });
                await revitTask.Run(() => { System.Console.WriteLine("3"); return string.Empty; });
                await revitTask.Run((uiapp) => { System.Console.WriteLine("4"); return string.Empty; });
            });

            return Result.Succeeded;
        }
    }

}
