using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks.Revit.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CommandTokenDelay : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIApplication uiapp = commandData.Application;

            var source = new CancellationTokenSource(500);
            var token = source.Token;

            var revitTask = App.RevitTask;

            try
            {
                var task = Task.Run(async () =>
                {
                    await Task.Delay(250);
                    await revitTask.Run(() => { System.Console.WriteLine("1"); }, token);
                });
                task.GetAwaiter().GetResult();
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
