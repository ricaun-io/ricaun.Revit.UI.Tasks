using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CommandTokenInContext : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            var source = new CancellationTokenSource(500);
            var token = source.Token;

            var revitTask = App.RevitTask;

            try
            {
                var task = revitTask.Run(() => { System.Console.WriteLine("1"); }, token);
                task.GetAwaiter().GetResult();
                var task2 = revitTask.Run(() => { System.Console.WriteLine("2"); Thread.Sleep(500); }, token);
                task2.GetAwaiter().GetResult();
                var task3 = revitTask.Run(() => { System.Console.WriteLine("3"); }, token);
                task3.GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                source.Dispose();
            }

            return Result.Succeeded;
        }
    }

}
