using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks.Revit
{
    [AppLoader]
    public class App : IExternalApplication
    {
        private static RibbonPanel ribbonPanel;
        private static RevitTaskService revitTaskService;
        public static IRevitTask RevitTask => revitTaskService;
        public Result OnStartup(UIControlledApplication application)
        {
            revitTaskService = new RevitTaskService(application);
            revitTaskService.Initialize();

            ribbonPanel = application.CreatePanel("Tasks");
            var button = ribbonPanel.CreatePushButton<Commands.Command>()
                .SetLargeImage("/UIFrameworkRes;component/ribbon/images/revit.ico");


            var task = Task.Run(async () =>
            {
                var source = new CancellationTokenSource(1500);
                var token = source.Token;

                try
                {
                    await Task.Delay(1000, token);

                    await RevitTask.Run(() =>
                    {
                        Thread.Sleep(1000);
                        button.SetText("Tasks");
                    }, token);

                    await RevitTask.Run(() =>
                    {
                        button.SetText("Tasks Canceled");
                    }, token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    source.Dispose();
                }

            });

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ribbonPanel?.Remove();
            revitTaskService?.Dispose();
            return Result.Succeeded;
        }
    }

}
